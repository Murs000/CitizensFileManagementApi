using System;

namespace CitizenFileManagement.Infrastructure.External.Services.MinIOService;

public interface IMinIOService
{
    public Task CreateUserBucketAsync(string userId);

    // Method to upload a document (with or without a file pack)
    public Task UploadDocumentAsync(string userId, string filePackName, string objectName, Stream fileStream);

    // Method to list all objects in a bucket (documents and file packs)
    public Task ListObjectsAsync(string userId);

    // Method to download a document
    public Task DownloadDocumentAsync(string userId, string filePackName, string objectName, string destinationPath);
}
