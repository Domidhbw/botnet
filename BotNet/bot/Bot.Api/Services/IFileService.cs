using Bot.Api.Models;

namespace Bot.Api.Services
{
    public interface IFileService
    {
        FileModel? ExecuteFileDownload(string command);
    }
}
