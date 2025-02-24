namespace CommandControlServer.Api.Models
{
    public class BotResponse
    {
        public int BotResponseId { get; set; }
        public int BotId { get; set; }
        public Bot? Bot { get; set; }
        public string ResponseType { get; set; } = "file";
        public string Data { get; set; } = string.Empty;
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public string FileName { get; set; } = string.Empty;
    }
}