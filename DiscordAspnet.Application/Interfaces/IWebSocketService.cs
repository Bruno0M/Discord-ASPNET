using System.Net.WebSockets;
using System.Security.Claims;

namespace DiscordAspnet.Application.Interfaces
{
    public interface IWebSocketService
    {
        Task OnConnectionReceived(WebSocket userConnection, Guid guildId, Guid channelId, ClaimsPrincipal user);

    }
}
