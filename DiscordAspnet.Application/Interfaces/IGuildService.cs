using DiscordAspnet.Application.DTOs;
using DiscordAspnet.Application.DTOs.GuildDTOs;
using DiscordAspnet.Domain.Entities;

namespace DiscordAspnet.Application.Interfaces
{
    public interface IGuildService
    {
        Task<ServiceResponse<GuildResponse>> CreateGuildsAsync(GuildRequest guildRequest);
        Task<ServiceResponse<IEnumerable<GuildResponse>>> GetGuildsAsync();
        Task DeleteGuildAsync(Guid guildId, Guid ownerId);
    }
}
