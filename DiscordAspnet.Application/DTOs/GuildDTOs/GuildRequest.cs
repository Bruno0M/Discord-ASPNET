namespace DiscordAspnet.Application.DTOs.GuildDTOs
{
    public record GuildRequest(
        string Name,
        Guid OwnerId);
}
