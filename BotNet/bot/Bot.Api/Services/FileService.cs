using Bot.Api.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace Bot.Api.Services
{
    public class FileService : IFileService
    {
        private readonly string _baseDirectory = @""; 

        public FileModel? ExecuteFileDownload(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return null;

            string fullPath = Path.Combine(_baseDirectory, relativePath);

            if (!File.Exists(fullPath)) return null;

            try
            {
                byte[] fileBytes = File.ReadAllBytes(fullPath);
                string fileName = Path.GetFileName(fullPath);

                // Get MIME type dynamically
                var provider = new FileExtensionContentTypeProvider();
                string contentType = provider.TryGetContentType(fullPath, out var mimeType) ? mimeType : "application/octet-stream";

                return new FileModel
                {
                    FileName = fileName,
                    FileContent = fileBytes,
                    ContentType = contentType
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
