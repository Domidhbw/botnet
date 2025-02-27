using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommandControlServer.Api.Controllers
{
    public class FileRequest
    {
        public List<int> BotIds { get; set; } = new();
        public string FilePath { get; set; } = string.Empty;
    }

    public class CommandRequest
    {
        public List<int> BotIds { get; set; } = new();
        public string Command { get; set; } = string.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public DataController(HttpClient httpClient, AppDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        [HttpPost("execute/file")]
        public async Task<IActionResult> FetchFile([FromBody] FileRequest request)
        {
            if (request.BotIds == null || !request.BotIds.Any()) return BadRequest("At least one bot ID is required.");
            if (string.IsNullOrWhiteSpace(request.FilePath)) return BadRequest("FilePath is required.");

            var bots = await _context.Bots.Where(b => request.BotIds.Contains(b.BotId)).ToListAsync();
            if (bots.Count < request.BotIds.Count)
            {
                var missingBotIds = request.BotIds.Except(bots.Select(b => b.BotId));
                return BadRequest($"Bot IDs not found: {string.Join(", ", missingBotIds)}");
            }

            var responses = new List<BotResponse>();
            foreach (var bot in bots)
            {
                bot.LastAction = DateTimeOffset.UtcNow;
                _context.Entry(bot).State = EntityState.Modified;

                try
                {
                    string url = $"http://{bot.DockerName}:8080/api/file/download?filepath={request.FilePath}";
                    var fileBytes = await _httpClient.GetByteArrayAsync(url);

                    var savePath = Path.Combine("BotFiles", bot.BotId.ToString());
                    Directory.CreateDirectory(savePath);
                    try
                    {
                        System.IO.File.SetAttributes(savePath, FileAttributes.Normal);
                    }
                    catch { }

                    var fileName = Path.GetFileName(request.FilePath);
                    var fullFilePath = Path.Combine(savePath, fileName);
                    await System.IO.File.WriteAllBytesAsync(fullFilePath, fileBytes);

                    responses.Add(new BotResponse
                    {
                        BotId = bot.BotId,
                        ResponseType = "file",
                        Success = true,
                        Timestamp = DateTimeOffset.UtcNow,
                        FilePath = fullFilePath,
                        FileName = fileName,
                        Command = request.FilePath
                    });
                }
                catch (Exception ex)
                {
                    responses.Add(new BotResponse
                    {
                        BotId = bot.BotId,
                        ResponseType = "file",
                        Success = false,
                        Timestamp = DateTimeOffset.UtcNow,
                        ResponseContent = $"Error: {ex.Message}",
                        Command = request.FilePath
                    });
                }
            }
            
            _context.BotResponses.AddRange(responses);
            await _context.SaveChangesAsync();

            var botResponseDtos = responses.Select(br => new BotResponseDto
            {
                BotResponseId = br.BotResponseId,
                BotId = br.BotId,
                ResponseType = br.ResponseType,
                Success = br.Success,
                Timestamp = br.Timestamp,
                FilePath = br.FilePath,
                FileName = br.FileName,
                ResponseContent = br.ResponseContent,
                Command = br.Command,
                Bot = new BotDto
                {
                    BotId = br.Bot.BotId,
                    Dockername = br.Bot.DockerName,
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

        [HttpPost("execute/command")]
        public async Task<IActionResult> RunCommand([FromBody] CommandRequest request)
        {
            if (request.BotIds == null || !request.BotIds.Any()) return BadRequest("At least one bot ID is required.");
            if (string.IsNullOrWhiteSpace(request.Command)) return BadRequest("Command is required.");

            var bots = await _context.Bots.Where(b => request.BotIds.Contains(b.BotId)).ToListAsync();
            if (bots.Count < request.BotIds.Count)
            {
                var missingBotIds = request.BotIds.Except(bots.Select(b => b.BotId));
                return BadRequest($"Bot IDs not found: {string.Join(", ", missingBotIds)}");
            }

            var responses = new List<BotResponse>();

            foreach (var bot in bots)
            {
                bot.LastAction = DateTimeOffset.UtcNow;
                _context.Entry(bot).State = EntityState.Modified;

                try
                {
                    string url = $"http://{bot.DockerName}:8080/api/command/run?cmd={request.Command}";
                    var responseContent = await _httpClient.GetStringAsync(url);
                    responses.Add(new BotResponse
                    {
                        BotId = bot.BotId,
                        ResponseType = "command",
                        Success = true,
                        Timestamp = DateTimeOffset.UtcNow,
                        ResponseContent = responseContent,
                        Command = request.Command
                    });
                }
                catch (Exception ex)
                {
                    responses.Add(new BotResponse
                    {
                        BotId = bot.BotId,
                        ResponseType = "command",
                        Success = false,
                        Timestamp = DateTimeOffset.UtcNow,
                        ResponseContent = $"Error: {ex.Message}",
                        Command = request.Command
                    });
                }
            }

            _context.BotResponses.AddRange(responses);
            await _context.SaveChangesAsync();

            var botResponseDtos = responses.Select(br => new BotResponseDto
            {
                BotResponseId = br.BotResponseId,
                BotId = br.BotId,
                ResponseType = br.ResponseType,
                Success = br.Success,
                Timestamp = br.Timestamp,
                FilePath = br.FilePath,
                FileName = br.FileName,
                ResponseContent = br.ResponseContent,
                Command = br.Command,
                Bot = new BotDto
                {
                    BotId = br.Bot.BotId,
                    Dockername = br.Bot.DockerName,
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

        [HttpGet("download")]
        public async Task<IActionResult> DownloadStoredFile([FromQuery] string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return NotFound("File path is required.");
            if (!System.IO.File.Exists(filePath)) return NotFound("File not found.");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);

            return File(fileBytes, "application/octet-stream", fileName);
        }

    }
}
