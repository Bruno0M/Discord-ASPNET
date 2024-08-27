using DiscordAspnet.Application.Interfaces;
using DiscordAspnet.Domain.Adapters;
using DiscordAspnet.Domain.Entities;
using System.Net.WebSockets;
using System.Security.Claims;

namespace DiscordAspnet.Application.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly IWebSocketAdapter _webSocketAdapter;

        public WebSocketService(IWebSocketAdapter webSocketAdapter)
        {
            _webSocketAdapter = webSocketAdapter;
        }

        public async Task OnConnectionReceived(WebSocket userConnection, Guid guildId, Guid channelId, ClaimsPrincipal user)
        {
            var channel = new Channel()
            {
                Id = channelId,
                GuildId = guildId
            };

            await _webSocketAdapter.HandleUser(userConnection, channel, user);
        }
    }
}
