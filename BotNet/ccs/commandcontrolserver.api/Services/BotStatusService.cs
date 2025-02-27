using CommandControlServer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandControlServer.Api.Services
{
    public class BotStatusService: IBotStatusService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public BotStatusService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task CheckAndRemoveOfflineBotsAsync()
        {
            var bots = await _context.Bots.ToListAsync();
            var botsToRemove = new List<Bot>();

            foreach (var bot in bots)
            {
                var url = $"http://{bot.DockerName}:8080/api/online/onlinestatus";
                try
                {
                    var response = await _httpClient.GetAsync(url);
                    Console.WriteLine(response.StatusCode.ToString(), response.Content.ReadAsStringAsync().Result);
                    if (!response.IsSuccessStatusCode)
                    {
                        botsToRemove.Add(bot);
                    }
                }
                catch
                {
                    botsToRemove.Add(bot);
                }
            }

            if (botsToRemove.Any())
            {
                _context.Bots.RemoveRange(botsToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
