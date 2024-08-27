using DiscordAspnet.Application.DTOs.ChannelDTOs;
using DiscordAspnet.Application.DTOs.MessageDTOs;
using DiscordAspnet.Domain.Adapters;
using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace DiscordAspnet.Infrastructure.Adapters
{
    public class WebSocketAdapter : IWebSocketAdapter
    {
        private readonly ConcurrentDictionary<ChannelRequest, ConcurrentDictionary<Guid, WebSocket>> _channelRoom = new();
        private readonly IServiceProvider _serviceProvider;

        public WebSocketAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task HandleUser(WebSocket userConnection, Channel channel, ClaimsPrincipal user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var _messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();

                var channelRequest = new ChannelRequest(channel.GuildId, channel.Id);
                var userConnections = _channelRoom.GetOrAdd(channelRequest, _ => new ConcurrentDictionary<Guid, WebSocket>());

                var userM = _userManager.GetUserAsync(user).Result;
                if (userM == null) return;

                if (userConnections.TryGetValue(userM.Id, out var existingConnection))
                {
                    userConnections.TryRemove(userM.Id, out _);
                }

                userConnections[userM.Id] = userConnection;

                var messageHistory = await _messageRepository.GetAllMessagesAsync(channel.Id);

                foreach (var message in messageHistory)
                {
                    var messageResponse = new MessageResponse(
                        message.Content,
                        message.User.UserName,
                        message.ChannelId,
                        message.CreatedAt);

                    await SendMessageAsync(userConnection, messageResponse);
                }

                var messageRequest = new MessageResponse($"{userM.UserName} Connected", userM.UserName, channelRequest.ChannelId, DateTime.Now);

                await BroadcastMessageToChannel(channelRequest, messageRequest, userConnection);
                await ReceiveMessagesAsync(userConnection, channelRequest, userM);
            }
        }

        private async Task SendMessageAsync(WebSocket userConnection, MessageResponse messageResponse)
        {
            var messageString = JsonSerializer.Serialize(messageResponse);
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageString));

            await userConnection.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task ReceiveMessagesAsync(WebSocket userConnection, ChannelRequest channelRequest, ApplicationUser user)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await userConnection.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var messageString = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {messageString}");

                var messageRequest = JsonSerializer.Deserialize<MessageRequest>(messageString);
                if (messageRequest == null) return;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var _messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();

                    var message = new Message()
                    {
                        Content = messageRequest.Content,
                        UserId = user.Id,
                        ChannelId = channelRequest.ChannelId,
                        CreatedAt = DateTime.UtcNow,
                    };

                    await _messageRepository.SaveMessageAsync(message);

                    var messageResponse = new MessageResponse(
                        message.Content,
                        user.UserName,
                        message.ChannelId,
                        message.CreatedAt);

                    await BroadcastMessageToChannel(channelRequest, messageResponse, userConnection);
                }


                result = await userConnection.ReceiveAsync(buffer, CancellationToken.None);
            }

            if (_channelRoom.TryGetValue(channelRequest, out var userConnections))
            {
                userConnections.TryRemove(user.Id, out _);

                if (userConnections.IsEmpty)
                {
                    _channelRoom.TryRemove(channelRequest, out _);
                }
            }

            await userConnection.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

        }

        private async Task BroadcastMessageToChannel(ChannelRequest channelRequest, MessageResponse messageResponse, WebSocket excludeConnection = null)
        {
            if (_channelRoom.TryGetValue(channelRequest, out var userConnections))
            {
                foreach (var connection in userConnections.Values)
                {
                    await SendMessageAsync(connection, messageResponse);
                }
            }
        }
    }
}