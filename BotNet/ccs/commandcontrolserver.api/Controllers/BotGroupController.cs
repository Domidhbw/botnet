using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CommandControlServer.Api.Controllers
{
    public class EditBotsDto
    {
        public List<int> BotIds { get; set; } = new();
    }

    [Route("api/[controller]")]
    [ApiController]
    public class BotGroupController : ControllerBase
    {
        public readonly AppDbContext _context;

        public BotGroupController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("botGroups")]
        public async Task<ActionResult<IEnumerable<BotGroupDto>>> GetBotGroups()
        {
            var botGroups = await _context.BotGroups
                .Include(bg => bg.Bots)
                .ThenInclude(b => b.Responses)
                .ToListAsync();

            var botGroupDtos = botGroups.Select(bg => new BotGroupDto
            {
                BotGroupId = bg.BotGroupId,
                Name = bg.Name,
                CreatedAt = bg.CreatedAt,
                Bots = bg.Bots.Select(b => new BotDto
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
                        Data = br.Data,
                        Timestamp = br.Timestamp,
                        FileName = br.FileName
                    }).ToList(),
                    BotGroups = b.BotGroups.Select(bg => new BotGroupDto
                    {
                        BotGroupId = bg.BotGroupId,
                        Name = bg.Name,
                        CreatedAt = bg.CreatedAt
                    }).ToList()
                }).ToList()
            }).ToList();
            return Ok(botGroupDtos);
        }

        [HttpPost("botGroup")]
        public async Task<ActionResult<BotGroup>> AddBotGroup([FromBody] BotGroup botGroup)
        {
            if (botGroup == null) return BadRequest("BotGroup is null");
            _context.BotGroups.Add(botGroup);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBotGroup", new { id = botGroup.BotGroupId }, botGroup);
        }

        [HttpGet("botGroup/{id}")]
        public async Task<ActionResult<BotGroupDto>> GetBotGroup(int id)
        {
            var botGroup = await _context.BotGroups
                .Include(bg => bg.Bots)
                .ThenInclude(b => b.Responses)
                .FirstOrDefaultAsync(bg => bg.BotGroupId == id);
            if (botGroup == null) return NotFound();

            var botGroupDto = new BotGroupDto
            {
                BotGroupId = botGroup.BotGroupId,
                Name = botGroup.Name,
                CreatedAt = botGroup.CreatedAt,
                Bots = botGroup.Bots.Select(b => new BotDto
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
                        Data = br.Data,
                        Timestamp = br.Timestamp,
                        FileName = br.FileName
                    }).ToList(),
                    BotGroups = b.BotGroups.Select(bg => new BotGroupDto
                    {
                        BotGroupId = bg.BotGroupId,
                        Name = bg.Name,
                        CreatedAt = bg.CreatedAt
                    }).ToList(),
                }).ToList()
            };

            return Ok(botGroupDto);
        }

        [HttpPut("botGroup/{id}")]
        public async Task<IActionResult> UpdateBotGroup(int id, [FromBody] BotGroup botGroup)
        {
            if (id != botGroup.BotGroupId) return BadRequest();
            _context.Entry(botGroup).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("editBots/{id}")]
        public async Task<IActionResult> EditBots(int id, [FromBody] EditBotsDto dto)
        {
            var botGroup = await _context.BotGroups
                .Include(bg => bg.Bots)
                .FirstOrDefaultAsync(bg => bg.BotGroupId == id);

            if (botGroup == null) return NotFound();

            var bots = await _context.Bots
                .Where(b => dto.BotIds.Contains(b.BotId))
                .ToListAsync();

            botGroup.Bots = bots;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("botGroup/{id}")]
        public async Task<IActionResult> DeleteBotGroup(int id)
        {
            var botGroup = await _context.BotGroups.FindAsync(id);
            if (botGroup == null) return NotFound();
            _context.BotGroups.Remove(botGroup);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}