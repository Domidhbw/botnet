using CommandControlServer.Api.Models;
using CommandControlServer.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CommandControlServer.Api.Controllers;
using System.IO.Compression;

namespace CommandControlServer.Api.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly IBotStatusService _botStatusService;

        public DataService(HttpClient httpClient, AppDbContext context, IBotStatusService botStatusService)
        {
            _httpClient = httpClient;
            _context = context;
            _botStatusService = botStatusService;
        }

        public async Task<List<BotResponseDto>> FetchFileAsync(FileRequest request)
        {
            if (request.BotIds == null || !request.BotIds.Any()) throw new ArgumentException("At least one bot ID is required.");
            if (string.IsNullOrWhiteSpace(request.FilePath)) throw new ArgumentException("FilePath is required.");

            await _botStatusService.CheckAndRemoveOfflineBotsAsync();
            var bots = await _context.Bots.Where(b => request.BotIds.Contains(b.BotId)).ToListAsync();
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
                    try { System.IO.File.SetAttributes(savePath, FileAttributes.Normal); } catch { }

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

            return responses.Select(MapToDto).ToList();
        }

        public async Task<List<BotResponseDto>> RunCommandAsync(CommandRequest request)
        {
            if (request.BotIds == null || !request.BotIds.Any()) throw new ArgumentException("At least one bot ID is required.");
            if (string.IsNullOrWhiteSpace(request.Command)) throw new ArgumentException("Command is required.");

            await _botStatusService.CheckAndRemoveOfflineBotsAsync();
            var bots = await _context.Bots.Where(b => request.BotIds.Contains(b.BotId)).ToListAsync();
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

            return responses.Select(MapToDto).ToList();
        }

        public async Task<FileStreamResult> FetchAndDownloadAsync(DownloadRequest request)
        {
            if (request.BotIds == null || !request.BotIds.Any()) throw new ArgumentException("At least one bot ID is required.");
            if (string.IsNullOrWhiteSpace(request.FilePath)) throw new ArgumentException("FilePath is required.");
            await _botStatusService.CheckAndRemoveOfflineBotsAsync();
            var bots = await _context.Bots.Where(b => request.BotIds.Contains(b.BotId)).ToListAsync();
            var responses = new List<BotResponse>();
            var zipPath = Path.Combine("BotFiles", "Download", Guid.NewGuid().ToString());
            Directory.CreateDirectory(zipPath);
            try { System.IO.File.SetAttributes(zipPath, FileAttributes.Normal); } catch { }
            foreach (var bot in bots)
            {
                bot.LastAction = DateTimeOffset.UtcNow;
                _context.Entry(bot).State = EntityState.Modified;
                try
                {
                    string url = $"http://{bot.DockerName}:8080/api/file/download?filepath={request.FilePath}";
                    var fileBytes = await _httpClient.GetByteArrayAsync(url);
                    var fileName = Path.GetFileName(request.FilePath);
                    var fullFilePath = Path.Combine(zipPath, $"{bot.BotId}_{fileName}");
                    await System.IO.File.WriteAllBytesAsync(fullFilePath, fileBytes);

                    var savePath = Path.Combine("BotFiles", bot.BotId.ToString());
                    Directory.CreateDirectory(savePath);
                    try { System.IO.File.SetAttributes(savePath, FileAttributes.Normal); } catch { }

                    var localFilePath = Path.Combine(savePath, fileName);
                    await System.IO.File.WriteAllBytesAsync(localFilePath, fileBytes);

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
            var zipFilePath = Path.Combine("BotFiles", "Download", $"{Guid.NewGuid()}.zip");
            ZipFile.CreateFromDirectory(zipPath, zipFilePath);
            var stream = new FileStream(zipFilePath, FileMode.Open);
            return new FileStreamResult(stream, "application/zip")
            {
                FileDownloadName = request.FilePath + ".zip"
            };
        }

        private static BotResponseDto MapToDto(BotResponse br)
        {
            return new BotResponseDto
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
                    DockerName = br.Bot.DockerName,
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
            };
        }
    }
    }
