using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;

namespace CommandControlServer.Api.Services
{
    public interface IBotService
    {
        Task<IEnumerable<BotDto>> GetBotsAsync();
        Task<BotDto?> GetBotByIdAsync(int id);
        Task<Bot?> RegisterBotAsync(string? remoteIp, int? remotePort);
        Task<bool> EditNameAsync(int id, string name);
        Task<bool> EditBotGroupsAsync(int id, List<int> botGroupIds);
        Task<bool> DeleteBotAsync(int id);
    }
}