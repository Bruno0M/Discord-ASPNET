namespace DiscordAspnet.Application.DTOs.GuildDTOs
{
    public record GuildResponse(
        Guid Id,
        string Name,
        Guid OwnerId,
        DateTime CreatedAt);
}
