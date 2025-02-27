using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;
using CommandControlServer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommandControlServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotResponseController : ControllerBase
    {
        private readonly BotResponseService _botResponseService;

        public BotResponseController(BotResponseService botResponseService)
        {
            _botResponseService = botResponseService;
        }

        [HttpGet("botResponses")]
        public async Task<ActionResult<IEnumerable<BotResponseDto>>> GetBotResponses()
        {
            return Ok(await _botResponseService.GetBotResponsesAsync());
        }

        [HttpPost("botResponse")]
        public async Task<ActionResult<BotResponseDto>> AddBotResponse([FromBody] BotResponse botResponse)
        {
            var createdBotResponse = await _botResponseService.AddBotResponseAsync(botResponse);
            return createdBotResponse == null
                ? BadRequest("Invalid BotResponse or Bot does not exist")
                : CreatedAtAction(nameof(GetBotResponse), new { id = createdBotResponse.BotResponseId }, createdBotResponse);
        }

        [HttpGet("botResponse/{id}")]
        public async Task<ActionResult<BotResponseDto>> GetBotResponse(int id)
        {
            var botResponse = await _botResponseService.GetBotResponseAsync(id);
            return botResponse == null ? NotFound() : Ok(botResponse);
        }
    }
}
