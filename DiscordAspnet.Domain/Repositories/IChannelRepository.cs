using DiscordAspnet.Domain.Entities;

namespace DiscordAspnet.Domain.Repositories
{
    public interface IChannelRepository
    {
        Task CreateChannelAsync(Channel channel);
        Task<bool> DeleteChannelAsync(Guid guildId, Guid channelId, Guid ownerId);
    }
}
