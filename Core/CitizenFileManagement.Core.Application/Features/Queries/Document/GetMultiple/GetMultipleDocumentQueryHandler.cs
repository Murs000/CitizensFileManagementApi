using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using System.IO.Compression;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;

public class GetMultipleDocumentQueryHandler : IRequestHandler<GetMultipleDocumentQuery, ReturnDocumentViewModel>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserManager _userManager;

    public GetMultipleDocumentQueryHandler(IDocumentRepository documentRepository, IUserManager userManager)
    {
        _documentRepository = documentRepository;
        _userManager = userManager;
    }

    public async Task<ReturnDocumentViewModel> Handle(GetMultipleDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var documents = await _documentRepository.GetAllAsync(u => u.CreatorId == userId &&  request.DocumentIds.Contains(u.Id));

        if (documents == null)
        {
            throw new NotFoundException("Document not found.");
        }

        using var memoryStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var document in documents)
            {
                var zipEntry = zipArchive.CreateEntry(document.Name, CompressionLevel.Fastest);
                using var entryStream = zipEntry.Open();
                var fileContent = await File.ReadAllBytesAsync(document.Path, cancellationToken);
                await entryStream.WriteAsync(fileContent, 0, fileContent.Length, cancellationToken);
            }
        }

        byte[] fileBytes = memoryStream.ToArray();

        return new ReturnDocumentViewModel
        {
            Name = "Files.zip",
            Type = "application/zip",
            Bytes = fileBytes
        };
    }
}