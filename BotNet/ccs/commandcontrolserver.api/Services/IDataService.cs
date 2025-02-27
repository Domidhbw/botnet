using CommandControlServer.Api.Controllers;
using CommandControlServer.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandControlServer.Api.Services
{
    public interface IDataService
    {
        Task<List<BotResponseDto>> FetchFileAsync(FileRequest request);
        Task<List<BotResponseDto>> RunCommandAsync(CommandRequest request);
        Task<FileStreamResult> FetchAndDownloadAsync(DownloadRequest request);
    }
}
