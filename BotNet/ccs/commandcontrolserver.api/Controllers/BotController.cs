using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Services;
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
        private readonly IBotService _botService;

        public BotController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpGet("bots")]
        public async Task<ActionResult<IEnumerable<BotDto>>> GetBots()
        {
<<<<<<< Updated upstream
            return Ok(await _botService.GetBotsAsync());
=======
            var bots = await _context.Bots
                .Include(b => b.BotGroups)
                .Include(b => b.Responses)
                .ToListAsync();

            var botDtos = bots.Select(b => new BotDto
            {
                BotId = b.BotId,
                Dockername = b.DockerName,
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
                }
                ).ToList()
            }).ToList();

            return Ok(botDtos);
        }

        [HttpPost("bot")]
        public async Task<ActionResult<Bot>> RegisterBot([FromBody] string data)
        {

            var remoteName = data;
            if (await _context.Bots.AnyAsync(b => b.DockerName == remoteName)) return BadRequest("Port already exists");
            Console.WriteLine($"Remote Name: {remoteName}");
            var bot = new Bot
            {
                DockerName = remoteName,
                Name = "",
                LastAction = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            _context.Bots.Add(bot);
            await _context.SaveChangesAsync();

            return Ok(bot);
>>>>>>> Stashed changes
        }

        [HttpGet("bot/{id}")]
        public async Task<ActionResult<BotDto>> GetBot(int id)
        {
            var bot = await _botService.GetBotByIdAsync(id);
            if (bot == null) return NotFound();
            return Ok(bot);
        }

<<<<<<< Updated upstream
        [HttpPost("bot")]
        public async Task<ActionResult<Bot>> RegisterBot()
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var remotePort = HttpContext.Connection.RemotePort;
=======
            var botDto = new BotDto
            {
                BotId = bot.BotId,
                Dockername = bot.DockerName,
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
>>>>>>> Stashed changes

            var bot = await _botService.RegisterBotAsync(remoteIp, remotePort);
            if (bot == null) return BadRequest("Port already exists");
            return Ok(bot);
        }

        [HttpPut("editName/{id}")]
        public async Task<IActionResult> EditName(int id, [FromBody] string name)
        {
            if (await _botService.EditNameAsync(id, name))
                return NoContent();

            return BadRequest("Name already exists or Bot not found");
        }

        [HttpPut("editBotGroups/{id}")]
        public async Task<IActionResult> EditBotGroups(int id, [FromBody] EditBotGroupsDto dto)
        {
            if (await _botService.EditBotGroupsAsync(id, dto.BotGroupIds))
                return NoContent();

            return NotFound();
        }

        [HttpDelete("bot/{id}")]
        public async Task<IActionResult> DeleteBot(int id)
        {
            if (await _botService.DeleteBotAsync(id))
                return NoContent();

            return NotFound();
        }
    }
}