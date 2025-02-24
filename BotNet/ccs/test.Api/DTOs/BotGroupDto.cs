namespace CommandControlServer.Api.DTOs
{
    public class BotGroupDto
    {
        public int BotGroupId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ICollection<BotDto> Bots { get; set; } = new List<BotDto>();
    }
}