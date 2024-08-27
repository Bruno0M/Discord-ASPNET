namespace DiscordAspnet.Application.DTOs.ChannelDTOs
{
    public record ChannelRequest(string Name, Guid GuildId, Guid ChannelId)
    {
        public ChannelRequest(Guid GuildId, Guid ChannelId) : this(null, GuildId, ChannelId) { }
    }
}