using Minio;
using Microsoft.Extensions.Options;
using CitizenFileManagement.Infrastructure.External.Settings;
using Minio.DataModel.Args;

namespace CitizenFileManagement.Infrastructure.External.Services.MinIOService;

public class MinIOService : IMinIOService
{
    private readonly MinioClient _minioClient;

    public MinIOService(IOptions<MinIOSettings> options)
    {
        var minioOptions = options.Value;

        _minioClient = (MinioClient?)new MinioClient()
            .WithEndpoint(minioOptions.Endpoint)
            .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
            .Build();
    }

    // Ensure bucket exists; create if not.
    public async Task EnsureBucketExistsAsync(string bucketName)
    {
        var found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
        }
    }

    // Upload a file
    public async Task UploadFileAsync(string objectName, Stream fileStream, string contentType,string bucketName)
    {
        await EnsureBucketExistsAsync(bucketName);

        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType)
        );
    }

    // Download a file
    public async Task<Stream> DownloadFileAsync(string objectName,string bucketName)
    {
        var memoryStream = new MemoryStream();
        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            })
        );
        memoryStream.Position = 0;
        return memoryStream;
    }

    // Delete a file
    public async Task DeleteFileAsync(string objectName,string bucketName)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
        );
    }
}
