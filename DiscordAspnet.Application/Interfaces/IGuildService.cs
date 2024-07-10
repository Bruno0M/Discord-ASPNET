using DiscordAspnet.Application.DTOs;
using DiscordAspnet.Application.DTOs.ChannelDTOs;
using DiscordAspnet.Application.DTOs.GuildDTOs;
using DiscordAspnet.Domain.Entities;

namespace DiscordAspnet.Application.Interfaces
{
    public interface IGuildService
    {
        Task<ServiceResponse<GuildResponse>> CreateGuildsAsync(GuildRequest guildRequest);
        Task<ServiceResponse<IEnumerable<GuildResponse>>> GetGuildsAsync();
        Task<ServiceResponse<GuildResponse>> DeleteGuildAsync(Guid guildId, Guid ownerId);
        Task<ServiceResponse<IEnumerable<ChannelResponse>>> GetChannelsGuildAsync(Guid guildId);
    }
}
