namespace CommandControlServer.Api.Models
{
    public class BotResponse
    {
        public int BotResponseId { get; set; }
        public int BotId { get; set; }
        public Bot? Bot { get; set; }

        public string ResponseType { get; set; }
        public bool Success { get; set; }

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public string Command {  get; set; }
        public string ResponseContent { get; set; } = string.Empty;
    }
}