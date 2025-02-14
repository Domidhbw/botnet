using CommandControlServer.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CommandControlServer.Api
{
    public class BotController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BotController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("bots")]
        public async Task<ActionResult<IEnumerable<Bot>>> GetBots()
        {
            return await _context.Bots
                .Include(b => b.Responses)
                .Include(b => b.BotGroups)
                .ToListAsync();
        }

        [HttpPost("registerBot")]
        public async Task<ActionResult<Bot>> RegisterBot(Bot bot)
        {
            _context.Bots.Add(bot);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBot", new { id = bot.BotId }, bot);
        }

        [HttpGet("bot/{id}")]
        public async Task<ActionResult<Bot>> GetBot(int id)
        {
            var bot = await _context.Bots.FindAsync(id);
            if (bot == null)
            {
                return NotFound();
            }
            return bot;
        }

        [HttpPut("bot/{id}")]
        public async Task<IActionResult> UpdateBot(int id, Bot bot)
        {
            if (id != bot.BotId)
            {
                return BadRequest();
            }
            _context.Entry(bot).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("bot/{id}")]
        public async Task<IActionResult> DeleteBot(int id)
        {
            var bot = await _context.Bots.FindAsync(id);
            if (bot == null)
            {
                return NotFound();
            }
            _context.Bots.Remove(bot);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("botResponse")]
        public async Task<ActionResult<BotResponse>> AddBotResponse(BotResponse botResponse)
        {
            _context.BotResponses.Add(botResponse);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBotResponse", new { id = botResponse.BotResponseId }, botResponse);
        }

        [HttpGet("botResponses")]
        public async Task<ActionResult<IEnumerable<BotResponse>>> GetBotResponses()
        {
            return await _context.BotResponses
                .Include(br => br.Bot)
                .ToListAsync();
        }

        [HttpGet("botResponse/{id}")]
        public async Task<ActionResult<BotResponse>> GetBotResponse(int id)
        {
            var botResponse = await _context.BotResponses.FindAsync(id);
            if (botResponse == null)
            {
                return NotFound();
            }
            return botResponse;
        }


        [HttpGet("botGroups")]
        public async Task<ActionResult<IEnumerable<BotGroup>>> GetBotGroups()
        {
            return await _context.BotGroups
                .Include(bg => bg.Bots)
                .ThenInclude(b => b.Responses)
                .ToListAsync();
        }

        [HttpPost("botGroup")]
        public async Task<ActionResult<BotGroup>> AddBotGroup(BotGroup botGroup)
        {
            _context.BotGroups.Add(botGroup);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBotGroup", new { id = botGroup.BotGroupId }, botGroup);
        }

        [HttpGet("botGroup/{id}")]
        public async Task<ActionResult<BotGroup>> GetBotGroup(int id)
        {
            var botGroup = await _context.BotGroups.FindAsync(id);
            if (botGroup == null)
            {
                return NotFound();
            }
            return botGroup;
        }

        [HttpPut("botGroup/{id}")]
        public async Task<IActionResult> UpdateBotGroup(int id, BotGroup botGroup)
        {
            if (id != botGroup.BotGroupId)
            {
                return BadRequest();
            }
            _context.Entry(botGroup).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}