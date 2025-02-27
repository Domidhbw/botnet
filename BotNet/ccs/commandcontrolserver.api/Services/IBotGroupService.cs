using CommandControlServer.Api.DTOs;
using CommandControlServer.Api.Models;

namespace CommandControlServer.Api.Services
{
    public interface IBotGroupService
    {
        Task<List<BotGroupDto>> GetBotGroupsAsync();
        Task<BotGroupDto?> GetBotGroupAsync(int id);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
        Task<BotGroup?> AddBotGroupAsync(BotGroup botGroup);
        Task<bool> EditNameAsync(int id, string name);
        Task<bool> EditBotsAsync(int id, List<int> botIds);
        Task<bool> DeleteBotGroupAsync(int id);
    }
}
