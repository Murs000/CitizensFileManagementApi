using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Common.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
using CitizenFileManagement.Infrastructure.External.Helpers;
using CitizenFileManagement.Infrastructure.External.Services.MinIOService;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.Get;

public class GetDocumentQueryHandler(IDocumentRepository documentRepository, 
    IUserManager userManager,
    IUserRepository userRepository,
    IMinIOService minIOService) : IRequestHandler<GetDocumentQuery, ReturnDocumentModel>
{

    public async Task<ReturnDocumentModel> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = userManager.GetCurrentUserId();
        var user = await userRepository.GetAsync(u => u.Id == userId);
        var document = await documentRepository.GetAsync(u => u.CreatorId == userId && u.Id == request.DocumentId);

        if (document == null)
        {
            throw new NotFoundException("Document not found.");
        }

        var file = await minIOService.DownloadFileAsync(document.Path,$"{user.Id}{user.Username}");
        byte[] bytes = [];

        using (MemoryStream memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);  // Copy the stream to MemoryStream
            bytes = memoryStream.ToArray();    // Convert MemoryStream to byte array
        }

        return new ReturnDocumentModel
        {
            Name = document.Name,
            Type = document.ContentType,
            Bytes = bytes
        };
    }
}