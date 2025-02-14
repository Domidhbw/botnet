namespace CommandControlServer.Api.Models
{
    public class Bot
    {
        public int BotId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<BotResponse> Responses { get; set; }
        public ICollection<BotGroup> BotGroups { get; set; }
    }
}