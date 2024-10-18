using System;

namespace CitizenFileManagement.Infrastructure.External.Helpers;

public static class FileExtensionHelper
{
    public static string GetExtension(string fileExtension)
    {
        return fileExtension switch
        {
            ".pdf" => "application/pdf",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".jpg" => "image/jpeg",
            ".png" => "image/png",
            // Add more file types as needed
            _ => "application/octet-stream"
        };
    }
}
