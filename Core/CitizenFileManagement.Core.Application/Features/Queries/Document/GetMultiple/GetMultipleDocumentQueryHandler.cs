using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Common.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using System.IO.Compression;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;
using CitizenFileManagement.Infrastructure.External.Helpers;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;

public class GetMultipleDocumentQueryHandler(IDocumentRepository documentRepository, 
    IUserManager userManager,
    IUserRepository userRepository,
    IMinIOService minIOService) : IRequestHandler<GetMultipleDocumentQuery, ReturnDocumentModel>
{

    public async Task<ReturnDocumentModel> Handle(GetMultipleDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = userManager.GetCurrentUserId();
        var user = await userRepository.GetAsync(u => u.Id == userId);
        var documents = await documentRepository.GetAllAsync(u => u.CreatorId == userId && request.DocumentIds.Contains(u.Id));

        if (documents == null || !documents.Any())
        {
            throw new NotFoundException("Documents not found.");
        }

        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var document in documents)
            {
                var filePath = document.Path; // Path from the database stored in MinIO
                var fileName = document.Name; // File name from your database

                // Retrieve the file from MinIO
                var fileStream = await minIOService.DownloadFileAsync(filePath, $"{user.Id}{user.Username}");
                if (fileStream == null)
                {
                    throw new NotFoundException($"File {fileName} not found in storage.");
                }

                fileName += FileExtensionHelper.GetExtension(document.ContentType); 

                // Add the document to the zip archive
                var zipEntry = zipArchive.CreateEntry(fileName, CompressionLevel.Fastest);
                using var entryStream = zipEntry.Open();
                
                // Copy content from MinIO file stream to the zip entry stream
                await fileStream.CopyToAsync(entryStream, cancellationToken);
            }
        }

        byte[] fileBytes = memoryStream.ToArray();

        return new ReturnDocumentModel
        {
            Name = "Files.zip", // Name of the zip file
            Type = "application/zip", // Content type for zip files
            Bytes = fileBytes // The byte array representing the zip file
        };
    }
}