using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CommandControlServer.Api.Controllers
{
    public class EditBotGroupsDto
    {
        public List<int> BotGroupIds { get; set; } = new();
    }

    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BotController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("bots")]
        public async Task<ActionResult<IEnumerable<BotDto>>> GetBots()
        {
            var bots = await _context.Bots
                .Include(b => b.BotGroups)
                .Include(b => b.Responses)
                .ToListAsync();

            var botDtos = bots.Select(b => new BotDto
            {
                BotId = b.BotId,
                Port = b.Port,
                Name = b.Name,
                Status = b.Status,
                LastSeen = b.LastSeen,
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
                }
                ).ToList()
            }).ToList();

            return Ok(botDtos);
        }

        [HttpPost("bot")]
        public async Task<ActionResult<Bot>> RegisterBot([FromBody] Bot bot)
        {
            if (bot == null) return BadRequest("Bot is null");
            if (await _context.Bots.AnyAsync(b => b.Name == bot.Name && b.Name != String.Empty)) return BadRequest("Name already exists");
            if (await _context.Bots.AnyAsync(b => b.Port == bot.Port)) return BadRequest("Port already exists");
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var remotePort = HttpContext.Connection.RemotePort;
            Console.WriteLine($"Remote IP: {remoteIp}, Remote Port: {remotePort}");
            _context.Bots.Add(bot);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBot", new { id = bot.BotId }, bot);
        }

        [HttpGet("bot/{id}")]
        public async Task<ActionResult<BotDto>> GetBot(int id)
        {
            var bot = await _context.Bots
                .Include(b => b.BotGroups)
                .Include(b => b.Responses)
                .FirstOrDefaultAsync(b => b.BotId == id);

            if (bot == null) return NotFound();

            var botDto = new BotDto
            {
                BotId = bot.BotId,
                Port = bot.Port,
                Name = bot.Name,
                Status = bot.Status,
                LastSeen = bot.LastSeen,
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

            return Ok(botDto);
        }

        [HttpPut("bot/{id}")]
        public async Task<IActionResult> UpdateBot(int id, [FromBody] Bot bot)
        {
            // TODO: Validation on update
            // Example Body: 
            if (id != bot.BotId) return BadRequest();
            if (!await _context.Bots.AnyAsync(b => b.BotId == id)) return NotFound();
            if (await _context.Bots.AnyAsync(b => b.Name == bot.Name && b.Name != String.Empty && b.BotId != id)) return BadRequest("Name already exists");
            if (await _context.Bots.AnyAsync(b => b.Port == bot.Port && b.BotId != id)) return BadRequest("Port already exists");

            _context.Entry(bot).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("bot/{id}")]
        public async Task<IActionResult> DeleteBot(int id)
        {
            // TODO: Update responses and bot groups
            var bot = await _context.Bots.FindAsync(id);
            if (bot == null) return NotFound();

            _context.Bots.Remove(bot);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("editBotGroups/{id}")]
        public async Task<IActionResult> EditBotGroups(int id, [FromBody] EditBotGroupsDto dto)
        {
            var bot = await _context.Bots
                .Include(b => b.BotGroups)
                .FirstOrDefaultAsync(b => b.BotId == id);

            if (bot == null) return NotFound();

            var botGroups = await _context.BotGroups
                .Where(bg => dto.BotGroupIds.Contains(bg.BotGroupId))
                .ToListAsync();

            bot.BotGroups = botGroups;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}