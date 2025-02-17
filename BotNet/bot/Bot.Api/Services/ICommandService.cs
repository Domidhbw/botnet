using Bot.Api.Models;

namespace Bot.Api.Services
{
    public interface ICommandService
    {
        Task<CommandResultModel> ExecuteCommandAsync(string command);
    }
}
