using System.Text.Json.Serialization;

namespace DiscordAspnet.Domain.Entities
{
    public class Guild
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public ApplicationUser Owner { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<ApplicationUser> Members { get; set; }
        public List<Channel> Channels { get; set; }
    }
}
