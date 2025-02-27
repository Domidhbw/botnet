using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandControlServer.Api.Services
{
    public class BotResponseService
    {
        private readonly AppDbContext _context;

        public BotResponseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BotResponseDto>> GetBotResponsesAsync()
        {
            var botResponses = await _context.BotResponses
                .Include(br => br.Bot)
                .ToListAsync();

            return botResponses.Select(MapToDto).ToList();
        }

        public async Task<BotResponseDto?> GetBotResponseAsync(int id)
        {
            var botResponse = await _context.BotResponses
                .Include(br => br.Bot)
                .FirstOrDefaultAsync(br => br.BotResponseId == id);

            return botResponse == null ? null : MapToDto(botResponse);
        }

        public async Task<BotResponseDto?> AddBotResponseAsync(BotResponse botResponse)
        {
            var bot = await _context.Bots.FindAsync(botResponse.BotId);
            if (bot == null) return null;

            _context.BotResponses.Add(botResponse);
            await _context.SaveChangesAsync();

            return MapToDto(botResponse);
        }

        private static BotResponseDto MapToDto(BotResponse br) => new()
        {
            BotResponseId = br.BotResponseId,
            BotId = br.BotId,
            ResponseType = br.ResponseType,
            Success = br.Success,
            Timestamp = br.Timestamp,
            FilePath = br.FilePath,
            FileName = br.FileName,
            Bot = new BotDto
            {
                BotId = br.Bot.BotId,
                DockerName = br.Bot.DockerName,
                Name = br.Bot.Name,
                LastAction = br.Bot.LastAction,
                CreatedAt = br.Bot.CreatedAt,
                UpdatedAt = br.Bot.UpdatedAt,
                BotGroups = br.Bot.BotGroups.Select(bg => new BotGroupDto
                {
                    BotGroupId = bg.BotGroupId,
                    Name = bg.Name,
                    CreatedAt = bg.CreatedAt
                }).ToList()
            }
        };
    }
}
