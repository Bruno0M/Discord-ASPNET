using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using DiscordAspnet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DiscordAspnet.Infrastructure.Repositories
{
    public class GuildRepository : IGuildRepository
    {
        private readonly AppDbContext _context;

        public GuildRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateGuildsAsync(Guild guild)
        {
            await _context.Guilds.AddAsync(guild);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGuildAsync(Guid guildId, Guid ownerId)
        {
            var guild = await _context.Guilds.FindAsync(guildId);
            if (guild != null && guild.OwnerId == ownerId)
            {
                _context.Guilds.Remove(guild);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Guild>> GetGuildsAsync()
        {
            return await _context.Guilds.AsNoTracking().ToListAsync();
        }
    }
}
