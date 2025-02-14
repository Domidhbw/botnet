namespace CommandControlServer.Api.Models
{
    public class BotResponse
    {
        public int BotResponseId { get; set; }
        public int BotId { get; set; }
        public Bot Bot { get; set; }
        public string ResponseType { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string FileName { get; set; }
    }
}