using Bot.Api.Models;

namespace Bot.Api.Services
{
    public interface ICommandService
    {
        Task<CommandResult> ExecuteCommandAsync(string command);
    }
}
