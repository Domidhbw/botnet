﻿using Bot.Api.Models;
using System.Diagnostics;
using System.Text;

namespace Bot.Api.Services
{
    //creates a terminal to execute the command
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
            process.BeginOutputReadLine();  
            process.BeginErrorReadLine();  

            await process.WaitForExitAsync();

            return new CommandResultModel
            {
                Command = command,
                Output = errorBuilder.Length > 0 ? errorBuilder.ToString() : outputBuilder.ToString()
            };
        }
    }
}
