namespace DiscordAspnet.Application.DTOs.ChannelDTOs
{
    public record ChannelResponse(
        Guid Id,
        Guid GuildId,
        string Name,
        int MessageCount);
}