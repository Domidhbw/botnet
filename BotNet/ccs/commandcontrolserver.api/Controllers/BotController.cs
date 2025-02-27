using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return Ok(await _botService.GetBotsAsync());
        }

        [HttpGet("bot/{id}")]
        public async Task<ActionResult<BotDto>> GetBot(int id)
        {
            var bot = await _botService.GetBotByIdAsync(id);
            if (bot == null) return NotFound();
            return Ok(bot);
        }

        [HttpPost("bot")]
        public async Task<ActionResult<Bot>> RegisterBot([FromBody] string data)
        {
            Console.WriteLine(data);
            var bot = await _botService.RegisterBotAsync(data);
            if (bot == null) return BadRequest("Port already exists");
            return Ok(bot);
        }

        [HttpPut("editName/{id}")]
        public async Task<IActionResult> EditName(int id, [FromBody] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name cannot be empty");
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