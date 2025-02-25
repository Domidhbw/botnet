namespace CommandControlServer.Api.DTOs
{
	public class BotResponseDto
    {
        public int BotResponseId { get; set; }
        public int BotId { get; set; }
        public BotDto? Bot { get; set; }

		public string ResponseType { get; set; }
		public bool Success { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }

        public string Command { get; set; }
        public string ResponseContent { get; set; }
    }
}