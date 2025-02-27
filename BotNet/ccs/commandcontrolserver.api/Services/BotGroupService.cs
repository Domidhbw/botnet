using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandControlServer.Api.Services
{
    public class BotGroupService: IBotGroupService
    {
        private readonly AppDbContext _context;

        public BotGroupService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BotGroupDto>> GetBotGroupsAsync()
        {
            var botGroups = await _context.BotGroups
                .Include(bg => bg.Bots)
                .ThenInclude(b => b.Responses)
                .ToListAsync();

            return botGroups.Select(MapToDto).ToList();
        }

        public async Task<BotGroupDto?> GetBotGroupAsync(int id)
        {
            var botGroup = await _context.BotGroups
                .Include(bg => bg.Bots)
                .ThenInclude(b => b.Responses)
                .FirstOrDefaultAsync(bg => bg.BotGroupId == id);

            return botGroup == null ? null : MapToDto(botGroup);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.BotGroups.AnyAsync(bg => bg.Name == name && (excludeId == null || bg.BotGroupId != excludeId));
        }

        public async Task<BotGroup?> AddBotGroupAsync(BotGroup botGroup)
        {
            if (string.IsNullOrWhiteSpace(botGroup.Name) || await NameExistsAsync(botGroup.Name)) return null;

            _context.BotGroups.Add(botGroup);
            await _context.SaveChangesAsync();
            return botGroup;
        }

        public async Task<bool> EditNameAsync(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || await NameExistsAsync(name, id)) return false;

            var botGroup = await _context.BotGroups.FindAsync(id);
            if (botGroup == null) return false;

            botGroup.Name = name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditBotsAsync(int id, List<int> botIds)
        {
            var botGroup = await _context.BotGroups
                .Include(bg => bg.Bots)
                .FirstOrDefaultAsync(bg => bg.BotGroupId == id);

            if (botGroup == null) return false;

            var bots = await _context.Bots.Where(b => botIds.Contains(b.BotId)).ToListAsync();
            botGroup.Bots = bots;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBotGroupAsync(int id)
        {
            var botGroup = await _context.BotGroups.FindAsync(id);
            if (botGroup == null) return false;

            _context.BotGroups.Remove(botGroup);
            await _context.SaveChangesAsync();
            return true;
        }

        private static BotGroupDto MapToDto(BotGroup bg) => new()
        {
            BotGroupId = bg.BotGroupId,
            Name = bg.Name,
            CreatedAt = bg.CreatedAt,
            Bots = bg.Bots.Select(b => new BotDto
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
                BotGroups = b.BotGroups.Select(g => new BotGroupDto
                {
                    BotGroupId = g.BotGroupId,
                    Name = g.Name,
                    CreatedAt = g.CreatedAt
                }).ToList()
            }).ToList()
        };
    }
}
