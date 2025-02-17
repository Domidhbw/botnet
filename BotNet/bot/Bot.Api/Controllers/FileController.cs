using Bot.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile([FromQuery] string filepath)
        {
            Console.WriteLine("[Method] DownloadFile");
            if (string.IsNullOrWhiteSpace(filepath))
            {
                return BadRequest("File path cannot be empty.");
            }

            var fileData = _fileService.ExecuteFileDownload(filepath);
            if (fileData == null)
            {
                return NotFound("File not found or access denied.");
            }

            return File(fileData.FileContent, fileData.ContentType, fileData.FileName);
        }
    }
}
