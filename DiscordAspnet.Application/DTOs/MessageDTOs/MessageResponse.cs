namespace DiscordAspnet.Application.DTOs.MessageDTOs
{
    public record MessageResponse(
        string Content,
        string Username,
        Guid ChannelId,
        DateTime CreatedAt);
}
