using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommandControlServer.Api.Services
{
    public class BotService : IBotService
    {
        private readonly AppDbContext _context;

        public BotService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BotDto>> GetBotsAsync()
        {
            var bots = await _context.Bots
                .Include(b => b.BotGroups)
                .Include(b => b.Responses)
                .ToListAsync();

            return bots.Select(b => new BotDto
            {
                BotId = b.BotId,
                DockerName = b.DockerName,
                Name = b.Name,
                LastAction = b.LastAction,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                Responses = b.Responses.Select(br => new BotResponseDto
                {
                    BotResponseId = br.BotResponseId,
                    BotId = br.BotId,
                    ResponseType = br.ResponseType,
                    Success = br.Success,
                    Timestamp = br.Timestamp,
                    FilePath = br.FilePath,
                    FileName = br.FileName
                }).ToList(),
                BotGroups = b.BotGroups.Select(bg => new BotGroupDto
                {
                    BotGroupId = bg.BotGroupId,
                    Name = bg.Name,
                    CreatedAt = bg.CreatedAt
                }).ToList()
            }).ToList();
        }

        public async Task<BotDto?> GetBotByIdAsync(int id)
        {
            var bot = await _context.Bots
                .Include(b => b.BotGroups)
                .Include(b => b.Responses)
                .FirstOrDefaultAsync(b => b.BotId == id);

            if (bot == null) return null;

            return new BotDto
            {
                BotId = bot.BotId,
                DockerName = bot.DockerName,
                Name = bot.Name,
                LastAction = bot.LastAction,
                CreatedAt = bot.CreatedAt,
                UpdatedAt = bot.UpdatedAt,
                Responses = bot.Responses.Select(br => new BotResponseDto
                {
                    BotResponseId = br.BotResponseId,
                    BotId = br.BotId,
                    ResponseType = br.ResponseType,
                    Success = br.Success,
                    Timestamp = br.Timestamp,
                    FilePath = br.FilePath,
                    FileName = br.FileName
                }).ToList(),
                BotGroups = bot.BotGroups.Select(bg => new BotGroupDto
                {
                    BotGroupId = bg.BotGroupId,
                    Name = bg.Name,
                    CreatedAt = bg.CreatedAt
                }).ToList()
            };
        }

        public async Task<Bot?> RegisterBotAsync(string data)
        {
            if (await _context.Bots.AnyAsync(b => b.DockerName == data)) return null;

            var bot = new Bot
            {
                DockerName = data,
                Name = "",
                LastAction = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _context.Bots.Add(bot);
            await _context.SaveChangesAsync();
            return bot;
        }

        public async Task<bool> EditNameAsync(int id, string name)
        {
            var bot = await _context.Bots.FindAsync(id);
            if (bot == null || (!String.IsNullOrEmpty(name) && _context.Bots.Any(b => b.Name == name && b.BotId != id)))
                return false;

            bot.Name = name;
            bot.UpdatedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditBotGroupsAsync(int id, List<int> botGroupIds)
        {
            var bot = await _context.Bots.Include(b => b.BotGroups).FirstOrDefaultAsync(b => b.BotId == id);
            if (bot == null) return false;

            var botGroups = await _context.BotGroups.Where(bg => botGroupIds.Contains(bg.BotGroupId)).ToListAsync();
            bot.BotGroups = botGroups;
            bot.UpdatedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBotAsync(int id)
        {
            var bot = await _context.Bots.FindAsync(id);
            if (bot == null) return false;

            _context.Bots.Remove(bot);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
