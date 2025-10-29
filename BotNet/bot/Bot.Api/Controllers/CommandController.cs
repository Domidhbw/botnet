using Bot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : Controller
    {
        private readonly ICommandService _commandService;

        public CommandController(ICommandService commandService)
        {
            _commandService = commandService;
        }

        [HttpGet("run")]
        public async Task<IActionResult> RunCommand([FromQuery] string cmd)
        {
            Console.WriteLine("[Method] RunCommand");
            if (string.IsNullOrWhiteSpace(cmd))
            {
                return BadRequest(new { error = "Command cannot be empty" });
            }
            var password = "Pa55word!";
            var result = await _commandService.ExecuteCommandAsync(cmd);
            return Ok(result);
        }
    }
}
