using DiscordAspnet.Application.DTOs.ChannelDTOs;
using DiscordAspnet.Application.DTOs.MessageDTOs;
using DiscordAspnet.Domain.Adapters;
using DiscordAspnet.Domain.Entities;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DiscordAspnet.Infrastructure.Adapters
{
    public class WebSocketAdapter : IWebSocketAdapter
    {
        private readonly ConcurrentDictionary<ChannelRequest, ConcurrentBag<WebSocket>> _channelRoom = new();
        public async Task HandleUser(WebSocket userConnection, Channel channel)
        {
            var channelRequest = new ChannelRequest(channel.GuildId, channel.Id);
            var channelConnection = _channelRoom.GetOrAdd(channelRequest, new ConcurrentBag<WebSocket>());
            channelConnection.Add(userConnection);

            var messageRequest = new MessageRequest("User Connected");

            await BroadcastMessageToChannel(channelRequest, messageRequest);
            await ReceiveMessagesAsync(userConnection, channelRequest);
        }

        private async Task SendMessageAsync(WebSocket userConnection, MessageRequest message)
        {
            var messageString = JsonSerializer.Serialize(message);
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageString));

            await userConnection.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task ReceiveMessagesAsync(WebSocket userConnection, ChannelRequest channelRequest)
        {
            var buffer = new byte[1024 * 2];

            while (userConnection.State == WebSocketState.Open)
            {
                var result = await userConnection.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await userConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by user", CancellationToken.None);
                    break;
                }

                var messageString = Encoding.UTF8.GetString(buffer[..result.Count]);

                var messageSerializer = JsonSerializer.Deserialize<MessageRequest>(messageString); 

                await BroadcastMessageToChannel(channelRequest, messageSerializer);

                result = await userConnection.ReceiveAsync(buffer, CancellationToken.None);
            }

            _channelRoom[channelRequest].TryTake(out _);
        }

        private async Task BroadcastMessageToChannel(ChannelRequest channelRequest, MessageRequest messageRequest)
        {
            if (_channelRoom.TryGetValue(channelRequest, out var channelRoom))
            {
                foreach (var member in channelRoom)
                {
                    await SendMessageAsync(member, messageRequest);
                }
            }
        }
    }
}
