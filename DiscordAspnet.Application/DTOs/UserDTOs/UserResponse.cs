namespace DiscordAspnet.Application.DTOs.UserDTOs
{
    public record UserResponse(
        Guid Id,
        string Username,
        int ServersCount,
        int MessageCount,
        DateTime CreatedAt);
}
