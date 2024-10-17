using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Common.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using System.IO.Compression;
using CitizenFileManagement.Core.Application.Features.Queries.Models;

namespace CitizenFileManagement.Core.Application.Features.Queries.FilePack.Get;

public class GetFilePackQueryHandler : IRequestHandler<GetFilePackQuery, ReturnDocumentModel>
{
    private readonly IFilePackRepository _filePackRepository;
    private readonly IUserManager _userManager;

    public GetFilePackQueryHandler(IFilePackRepository filePackRepository, IUserManager userManager)
    {
        _filePackRepository = filePackRepository;
        _userManager = userManager;
    }

    public async Task<ReturnDocumentModel> Handle(GetFilePackQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var filePack = await _filePackRepository.GetAsync(u => u.CreatorId == userId &&  u.Id == request.FilePackId, "Files");

        if (filePack == null)
        {
            throw new NotFoundException("FilePack not found.");
        }

        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var file in filePack.Files)
            {
                var filePath = file.Path; // Full path of the document
                var fileName = file.Name; // File name from your database, including the extension
                var fileExtencion = Path.GetExtension(filePath);

                if (!File.Exists(filePath))
                {
                    throw new NotFoundException($"File {fileName} not found on the server.");
                }

                // Add the document to the zip with its original name and extension
                var zipEntry = zipArchive.CreateEntry(fileName + fileExtencion, CompressionLevel.Fastest);
                using var entryStream = zipEntry.Open();

                var fileContent = await File.ReadAllBytesAsync(filePath, cancellationToken);
                await entryStream.WriteAsync(fileContent, 0, fileContent.Length, cancellationToken);
            }
        }

        byte[] fileBytes = memoryStream.ToArray();

        return new ReturnDocumentModel
        {
            Name = $"{filePack.Name}.zip", // Name of the zip file
            Type = "application/zip", // Content type for zip files
            Bytes = fileBytes // The byte array representing the zip file
        };
    }
}