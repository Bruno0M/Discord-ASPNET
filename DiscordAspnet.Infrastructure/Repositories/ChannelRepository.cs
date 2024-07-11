using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using DiscordAspnet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DiscordAspnet.Infrastructure.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly AppDbContext _context;

        public ChannelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateChannelAsync(Channel channel)
        {
            await _context.Channels.AddAsync(channel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteChannelAsync(Guid guildId, Guid channelId, Guid ownerId)
        {
            var channel = await _context.Channels
                  .Include(c => c.Guilds)
                  .FirstOrDefaultAsync(c => c.Id == channelId && c.GuildId == guildId);

            if (channel != null && channel.Guilds.OwnerId == ownerId)
            {
                _context.Channels.Remove(channel);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
