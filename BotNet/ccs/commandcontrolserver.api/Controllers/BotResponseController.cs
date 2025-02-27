using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CommandControlServer.Api.Controllers
{
    public class BotResponseCreateDto
    {
        public int BotId { get; set; }
        public string ResponseType { get; set; } = "file";
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class BotResponseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BotResponseController(AppDbContext context) {
            _context = context;
        }

        [HttpGet("botResponses")]
        public async Task<ActionResult<IEnumerable<BotResponseDto>>> GetBotResponses()
        {
            var botResponses = await _context.BotResponses
                .Include(br => br.Bot)
                .ToListAsync();

            var botResponseDtos = botResponses.Select(br => new BotResponseDto
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
            }).ToList();

            return Ok(botResponseDtos);
        }

        [HttpPost("botResponse")]
        public async Task<ActionResult<BotResponseDto>> AddBotResponse([FromBody] BotResponse botResponse)
        {
            if (botResponse == null) return BadRequest("BotResponse is null");
            var bot = await _context.Bots.FindAsync(botResponse.BotId);
            if (bot == null) return BadRequest("Bot does not exist");
            _context.BotResponses.Add(botResponse);
            await _context.SaveChangesAsync();

            var botResponseDto = new BotResponseDto
            {
                BotResponseId = botResponse.BotResponseId,
                BotId = botResponse.BotId,
                ResponseType = botResponse.ResponseType,
                Success = botResponse.Success,
                Timestamp = botResponse.Timestamp,
                FilePath = botResponse.FilePath,
                FileName = botResponse.FileName,
                Bot = new BotDto
                {
                    BotId = botResponse.Bot.BotId,
                    DockerName = botResponse.Bot.DockerName,
                    Name = botResponse.Bot.Name,
                    LastAction = botResponse.Bot.LastAction,
                    CreatedAt = botResponse.Bot.CreatedAt,
                    UpdatedAt = botResponse.Bot.UpdatedAt,
                    BotGroups = botResponse.Bot.BotGroups.Select(bg => new BotGroupDto
                    {
                        BotGroupId = bg.BotGroupId,
                        Name = bg.Name,
                        CreatedAt = bg.CreatedAt
                    }).ToList()
                }
            };

            return CreatedAtAction("GetBotResponse", new { id = botResponse.BotResponseId }, botResponseDto);
        }

        [HttpGet("botResponse/{id}")]
        public async Task<ActionResult<BotResponseDto>> GetBotResponse(int id)
        {
            var botResponse = await _context.BotResponses
                .Include(br => br.Bot)
                .FirstOrDefaultAsync(br => br.BotResponseId == id);

            if (botResponse == null) return NotFound();

            var botResponseDto = new BotResponseDto
            {
                BotResponseId = botResponse.BotResponseId,
                BotId = botResponse.BotId,
                ResponseType = botResponse.ResponseType,
                Success = botResponse.Success,
                Timestamp = botResponse.Timestamp,
                FilePath = botResponse.FilePath,
                FileName = botResponse.FileName,
                Bot = new BotDto
                {
                    BotId = botResponse.Bot.BotId,
                    DockerName = botResponse.Bot.DockerName,
                    Name = botResponse.Bot.Name,
                    LastAction = botResponse.Bot.LastAction,
                    CreatedAt = botResponse.Bot.CreatedAt,
                    UpdatedAt = botResponse.Bot.UpdatedAt,
                    BotGroups = botResponse.Bot.BotGroups.Select(bg => new BotGroupDto
                    {
                        BotGroupId = bg.BotGroupId,
                        Name = bg.Name,
                        CreatedAt = bg.CreatedAt
                    }).ToList()
                }
            };

            return Ok(botResponseDto);
        }
    }
}