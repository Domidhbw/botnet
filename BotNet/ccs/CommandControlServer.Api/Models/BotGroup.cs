namespace CommandControlServer.Api.Models
{
    public class BotGroup
    {
        public int BotGroupId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Bot> Bots { get; set; }
    }
}