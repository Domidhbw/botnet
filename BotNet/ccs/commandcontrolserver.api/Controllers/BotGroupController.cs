using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;
using CommandControlServer.Api.Services;
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
        private readonly BotGroupService _botGroupService;

        public BotGroupController(BotGroupService botGroupService)
        {
            _botGroupService = botGroupService;
        }

        [HttpGet("botGroups")]
        public async Task<ActionResult<IEnumerable<BotGroupDto>>> GetBotGroups()
        {
            return Ok(await _botGroupService.GetBotGroupsAsync());
        }

        [HttpPost("botGroup")]
        public async Task<ActionResult<BotGroup>> AddBotGroup([FromBody] BotGroup botGroup)
        {
            var createdBotGroup = await _botGroupService.AddBotGroupAsync(botGroup);
            return createdBotGroup == null
                ? BadRequest("Invalid or duplicate BotGroup")
                : CreatedAtAction(nameof(GetBotGroup), new { id = createdBotGroup.BotGroupId }, createdBotGroup);
        }

        [HttpGet("botGroup/{id}")]
        public async Task<ActionResult<BotGroupDto>> GetBotGroup(int id)
        {
            var botGroup = await _botGroupService.GetBotGroupAsync(id);
            return botGroup == null ? NotFound() : Ok(botGroup);
        }

        [HttpPut("editName/{id}")]
        public async Task<IActionResult> EditName(int id, [FromBody] string name)
        {
            return await _botGroupService.EditNameAsync(id, name) ? NoContent() : BadRequest("Invalid or duplicate name");
        }

        [HttpPut("editBots/{id}")]
        public async Task<IActionResult> EditBots(int id, [FromBody] EditBotsDto dto)
        {
            return await _botGroupService.EditBotsAsync(id, dto.BotIds) ? NoContent() : NotFound();
        }

        [HttpDelete("botGroup/{id}")]
        public async Task<IActionResult> DeleteBotGroup(int id)
        {
            return await _botGroupService.DeleteBotGroupAsync(id) ? NoContent() : NotFound();
        }
    }
}
