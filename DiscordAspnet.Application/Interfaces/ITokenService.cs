using Microsoft.AspNetCore.Identity;

namespace DiscordAspnet.Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(IdentityUser<Guid> user);

    }
}
