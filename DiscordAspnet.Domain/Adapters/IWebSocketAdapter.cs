using DiscordAspnet.Domain.Entities;
using System.Net.WebSockets;
using System.Security.Claims;

namespace DiscordAspnet.Domain.Adapters
{
    public interface IWebSocketAdapter
    {
        Task HandleUser(WebSocket userConnection, Channel channel, ClaimsPrincipal user);
    }
}
