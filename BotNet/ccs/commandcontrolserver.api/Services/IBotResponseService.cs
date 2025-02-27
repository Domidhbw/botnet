using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;

namespace CommandControlServer.Api.Services
{
    public interface IBotResponseService
    {
        Task<List<BotResponseDto>> GetBotResponsesAsync();
        Task<BotResponseDto?> GetBotResponseAsync(int id);
        Task<BotResponseDto?> AddBotResponseAsync(BotResponse botResponse);
    }
}
