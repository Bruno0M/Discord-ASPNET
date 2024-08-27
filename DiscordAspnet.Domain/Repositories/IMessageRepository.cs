using DiscordAspnet.Domain.Entities;

namespace DiscordAspnet.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task SaveMessageAsync(Message message);
        Task<IEnumerable<Message>> GetAllMessagesAsync(Guid channelId);
    }
}
