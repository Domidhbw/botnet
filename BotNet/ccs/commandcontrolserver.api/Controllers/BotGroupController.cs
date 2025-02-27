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
                }).ToList()
            }).ToList();
            return Ok(botGroupDtos);
        }

        [HttpPost("botGroup")]
        public async Task<ActionResult<BotGroup>> AddBotGroup([FromBody] BotGroup botGroup)
        {
            if (botGroup == null) return BadRequest("BotGroup is null");
            if (string.IsNullOrEmpty(botGroup.Name)) return BadRequest("BotGroup name is required");
            if (await _context.BotGroups.AnyAsync(bg => bg.Name == botGroup.Name)) return BadRequest("BotGroup name already exists");
            
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
                    }).ToList(),
                }).ToList()
            };

            return Ok(botGroupDto);
        }

        [HttpPut("editName/{id}")]
        public async Task<IActionResult> EditName(int id, [FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required");
            if (await _context.BotGroups.AnyAsync(bg => bg.Name == name && bg.BotGroupId != id)) return BadRequest("Name already exists");

            var botGroup = await _context.BotGroups.FindAsync(id);
            if (botGroup == null) return NotFound();
            
            botGroup.Name = name;
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