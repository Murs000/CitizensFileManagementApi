using System;

namespace CitizenFileManagement.Infrastructure.External.Helpers;

public static class FileExtensionHelper
{
    public static string GetContentType(string fileExtension)
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
    public static string GetExtension(string fileExtension)
    {
        return fileExtension switch
        {
            "application/pdf" => ".pdf",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ".docx",
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            // Add more file types as needed
            _ => ".txt"
        };
    }
}
