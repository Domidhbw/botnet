namespace CommandControlServer.Api.Models
{
    public class Bot
    {
        public int BotId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "online";
        public DateTimeOffset LastSeen { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<BotResponse> Responses { get; set; } = new List<BotResponse>();
        public ICollection<BotGroup> BotGroups { get; set; } = new List<BotGroup>();
    }
}