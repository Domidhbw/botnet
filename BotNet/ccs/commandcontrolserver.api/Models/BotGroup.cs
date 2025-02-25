namespace CommandControlServer.Api.Models
{
    public class BotGroup
    {
        public int BotGroupId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<Bot> Bots { get; set; } = new List<Bot>();
    }
}