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
            
            // 1) Simple hardcoded password (generic credential rule)
            private const string DemoPassword = "SuperSecret123!";

        // 2) Connection string with Password= (db rules)
            private const string ConnString = "Server=localhost;Database=demo;User Id=sa;Password=MyS3cret!;";

        // 3) GitHub token pattern (matches ghp_ + 36 chars)
        private const string GitHubToken = "ghp_0123456789ABCdef0123456789ABCdef0123";

        // 4) AWS key pair (common rule); obviously fake
        private const string AwsAccessKeyId     = "AKIA1234567890ABCD12";
        private const string AwsSecretAccessKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";

            var result = await _commandService.ExecuteCommandAsync(cmd);
            return Ok(result);
        }
    }
}
