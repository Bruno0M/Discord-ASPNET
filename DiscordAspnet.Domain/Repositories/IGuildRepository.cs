using DiscordAspnet.Domain.Entities;

namespace DiscordAspnet.Domain.Repositories
{
    public interface IGuildRepository
    {
        Task CreateGuildsAsync(Guild guild);
        Task<IEnumerable<Guild>> GetGuildsAsync();
        Task<bool> DeleteGuildAsync(Guid guildId, Guid ownerId);
        Task<IEnumerable<Channel>> GetChannelsGuildAsync(Guid guildId);
    }
}
