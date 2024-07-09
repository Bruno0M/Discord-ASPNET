using System.Text.Json.Serialization;

namespace DiscordAspnet.Domain.Entities
{
    public class Channel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GuildId { get; set; }
        [JsonIgnore]
        public Guild Guilds { get; set; }
        public int MessageCount { get; set; }

        public List<Message> Messages { get; set; }
    }
}
