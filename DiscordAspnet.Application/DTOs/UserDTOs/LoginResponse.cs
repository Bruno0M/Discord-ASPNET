namespace DiscordAspnet.Application.DTOs.UserDTOs
{
    public record LoginResponse(
        Guid Id,
        string Username,
        DateTime CreatedAt,
        string Token);
}
