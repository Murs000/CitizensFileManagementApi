using System;

namespace CitizenFileManagement.Infrastructure.External.Services.MinIOService;

public interface IMinIOService
{
    // Ensure bucket exists; create if not.
    public Task EnsureBucketExistsAsync(string bucketName);

    // Upload a file
    public Task UploadFileAsync(string objectName, Stream fileStream, string contentType,string bucketName);

    // Download a file
    public Task<Stream> DownloadFileAsync(string objectName,string bucketName);
    // Delete a file
    public Task DeleteFileAsync(string objectName,string bucketName);
}
