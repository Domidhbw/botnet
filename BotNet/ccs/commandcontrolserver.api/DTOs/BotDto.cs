using CommandControlServer.Api.Models;

namespace CommandControlServer.Api.DTOs
{
    public class BotDto
    {
        public int BotId { get; set; }
        public string DockerName { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LastAction { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public ICollection<BotGroupDto> BotGroups { get; set; } = new List<BotGroupDto>();
        public ICollection<BotResponseDto> Responses { get; set; } = new List<BotResponseDto>();
    }
}