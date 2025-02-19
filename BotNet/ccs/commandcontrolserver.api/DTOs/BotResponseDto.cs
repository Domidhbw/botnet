namespace CommandControlServer.Api.DTOs
{
	public class BotResponseDto
	{
		public int BotResponseId { get; set; }
		public string ResponseType { get; set; }
		public string Data { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public string FileName { get; set; }
		public int BotId { get; set; }
        public BotDto? Bot { get; set; }
    }
}