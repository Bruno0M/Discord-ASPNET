using DiscordAspnet.Domain.Entities;

namespace DiscordAspnet.Domain.Repositories
{
    public interface IGuildRepository
    {
        Task CreateGuildsAsync(Guild guild);
        Task<IEnumerable<Guild>> GetGuildsAsync();
        Task DeleteGuildAsync(Guid guildId, Guid ownerId);
    }
}
