namespace DiscordAspnet.Application.DTOs.ChannelDTOs
{
    public record ChannelRequest(string Name, Guid GuildId, Guid ChannelId)
    {
        /// <summary>
        /// Diferencia os Channels de uma Guilda
        /// </summary>
        /// <param name="GuildId"></param>
        /// <param name="ChannelId"></param>
        public ChannelRequest(Guid GuildId, Guid ChannelId) : this(string.Empty, ChannelId, GuildId) { }
    }
}