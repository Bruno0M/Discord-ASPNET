using DiscordAspnet.Domain.Entities;
using System.Net.WebSockets;

namespace DiscordAspnet.Domain.Adapters
{
    public interface IWebSocketAdapter
    {
        Task HandleUser(WebSocket userConnection, Channel channel);
    }
}
