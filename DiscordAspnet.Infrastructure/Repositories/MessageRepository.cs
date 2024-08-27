using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using DiscordAspnet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DiscordAspnet.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync(Guid channelId)
        {
            return await _context.Messages
                .Include(m => m.User)
                .AsNoTracking()
                .Where(m => m.ChannelId == channelId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task SaveMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
    }
}
