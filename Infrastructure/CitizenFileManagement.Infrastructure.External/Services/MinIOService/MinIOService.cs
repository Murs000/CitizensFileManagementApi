using Minio;
using Minio.DataModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CitizenFileManagement.Infrastructure.External.Services.MinIOService;

public class MinioService : IMinIOService
{
    private readonly MinioClient _minioClient;

    public MinioService(string endpoint, string accessKey, string secretKey)
    {
        // _minioClient = new MinioClient()
        //     .WithEndpoint(endpoint)
        //     .WithCredentials(accessKey, secretKey)
        //     .Build();
    }

    // Method to create a user bucket
    public async Task CreateUserBucketAsync(string userId)
    {
        // string bucketName = userId.ToLower();
        // bool bucketExists = await _minioClient.BucketExistsAsync(bucketName);
        // if (!bucketExists)
        // {
        //     await _minioClient.MakeBucketAsync(bucketName);
        //     Console.WriteLine($"Bucket '{bucketName}' created for user.");
        // }
    }

    // Method to upload a document (with or without a file pack)
    public async Task UploadDocumentAsync(string userId, string filePackName, string objectName, Stream fileStream)
    {
        // string bucketName = userId.ToLower();
        // string objectPath = string.IsNullOrEmpty(filePackName) 
        //     ? objectName                // For free documents
        //     : $"{filePackName}/{objectName}"; // For documents inside file packs
        
        // // Upload file to MinIO
        // await _minioClient.PutObjectAsync(new PutObjectArgs()
        //     .WithBucket(bucketName)
        //     .WithObject(objectPath)
        //     .WithStreamData(fileStream)
        //     .WithObjectSize(fileStream.Length));

        // Console.WriteLine($"Document '{objectName}' uploaded to '{bucketName}/{objectPath}'.");
    }

    // Method to list all objects in a bucket (documents and file packs)
    public async Task ListObjectsAsync(string userId)
    {
        // string bucketName = userId.ToLower();
        // IObservable<Item> observable = _minioClient.ListObjectsAsync(bucketName, "", true);
        // observable.Subscribe(
        //     item => Console.WriteLine($"Object found: {item.Key}"),
        //     ex => Console.WriteLine($"Error: {ex.Message}"),
        //     () => Console.WriteLine("Object listing completed.")
        //);
    }

    // Method to download a document
    public async Task DownloadDocumentAsync(string userId, string filePackName, string objectName, string destinationPath)
    {
        // string bucketName = userId.ToLower();
        // string objectPath = string.IsNullOrEmpty(filePackName)
        //     ? objectName                // For free documents
        //     : $"{filePackName}/{objectName}"; // For file pack documents

        // // Download file from MinIO
        // await _minioClient.GetObjectAsync(new GetObjectArgs()
        //     .WithBucket(bucketName)
        //     .WithObject(objectPath)
        //     .WithFile(destinationPath));
        
        // Console.WriteLine($"Document '{objectName}' downloaded from '{bucketName}/{objectPath}'.");
    }
}
