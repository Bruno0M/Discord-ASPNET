using Microsoft.AspNetCore.Identity;

namespace DiscordAspnet.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public int ServersCount { get; set; }
        public int MessageCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
