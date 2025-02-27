namespace CommandControlServer.Api.Services
{
    public interface IBotStatusService
    {
        Task CheckAndRemoveOfflineBotsAsync();
    }
}
