using DiscordAspnet.Domain.Entities;

namespace DiscordAspnet.Domain.Repositories
{
    public interface IChannelRepository
    {
        Task CreateChannelAsync(Channel channel);
        Task<List<Channel>> GetChannelsGuildAsync(Guid guildId);
    }
}
