using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using System.IO.Compression;
using CitizenFileManagement.Core.Application.Features.Queries.Models;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;

public class GetByTypeDocumentQueryHandler : IRequestHandler<GetByTypeDocumentQuery, ReturnDocumentModel>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserManager _userManager;

    public GetByTypeDocumentQueryHandler(IDocumentRepository documentRepository, IUserManager userManager)
    {
        _documentRepository = documentRepository;
        _userManager = userManager;
    }

    public async Task<ReturnDocumentModel> Handle(GetByTypeDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var documents = await _documentRepository.GetAllAsync(u => u.CreatorId == userId && u.Type == request.Type);

        if (documents == null || !documents.Any())
        {
            throw new NotFoundException("Documents not found.");
        }

        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var document in documents)
            {
                var filePath = document.Path; // Full path of the document
                var fileName = document.Name; // File name from your database, including the extension
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
            Name = "Files.zip", // Name of the zip file
            Type = "application/zip", // Content type for zip files
            Bytes = fileBytes // The byte array representing the zip file
        };
    }
}