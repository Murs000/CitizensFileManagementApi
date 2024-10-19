using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CitizenFileManagement.Infrastructure.External.Extensions
{
    public static class IFormFileExtensions
    {
        public static async Task<string> SaveAsync(this IFormFile file, string basePath, string username, string? filepack)
        {
            string userFolderPath = string.Empty;
            
            if (filepack != null)
            {
                userFolderPath = Path.Combine(basePath, username, filepack);
            }
            else
            {
                userFolderPath = Path.Combine(basePath, username);
            }
            

            if (!Directory.Exists(userFolderPath))
            {
                Directory.CreateDirectory(userFolderPath);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            string fullPath = Path.Combine(userFolderPath, uniqueFileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fullPath;
        }
    }
}