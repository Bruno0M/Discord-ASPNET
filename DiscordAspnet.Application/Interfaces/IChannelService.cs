using DiscordAspnet.Application.DTOs;
using DiscordAspnet.Application.DTOs.ChannelDTOs;
using System.Net.WebSockets;

namespace DiscordAspnet.Application.Interfaces
{
    public interface IChannelService
    {
        Task<ServiceResponse<ChannelResponse>> CreateChannelAsync(ChannelRequest channelRequest, Guid guildId);
        Task<ServiceResponse<ChannelResponse>> DeleteChannelAsync(Guid guildId, Guid channelId, Guid ownerId);
        Task OnConnectionReceived(WebSocket userConnection, Guid guildId, Guid channelId);

    }
}
