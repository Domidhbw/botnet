using Bot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnlineController : Controller
    {

        [HttpGet("onlineStatus")]
        public async Task<IActionResult> getOnlineStatus()
        {
            Console.WriteLine("[Method] checkOnlineStatus");

            return Ok("online");
        }
    }
}
