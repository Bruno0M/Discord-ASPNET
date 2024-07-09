namespace DiscordAspnet.Application.DTOs.UserDTOs
{
    public record LoginRequest(
        string Credential,
        string Password);
}
