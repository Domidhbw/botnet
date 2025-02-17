using Bot.Api.Models;
using System.Diagnostics;
using System.Text;

namespace Bot.Api.Services
{
    public class CommandService : ICommandService
    {
        public async Task<CommandResultModel> ExecuteCommandAsync(string command)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "/bin/sh", 
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processStartInfo };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();  // Asynchronously reads output
            process.BeginErrorReadLine();   // Asynchronously reads errors

            await process.WaitForExitAsync(); // Waits for process completion

            return new CommandResultModel
            {
                Command = command,
                Output = errorBuilder.Length > 0 ? errorBuilder.ToString() : outputBuilder.ToString()
            };
        }
    }
}
