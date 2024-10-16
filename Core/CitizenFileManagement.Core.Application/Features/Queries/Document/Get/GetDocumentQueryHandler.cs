using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Common.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
using CitizenFileManagement.Infrastructure.External.Helpers;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.Get;

public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, ReturnDocumentModel>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserManager _userManager;

    public GetDocumentQueryHandler(IDocumentRepository documentRepository, IUserManager userManager)
    {
        _documentRepository = documentRepository;
        _userManager = userManager;
    }

    public async Task<ReturnDocumentModel> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var document = await _documentRepository.GetAsync(u => u.CreatorId == userId && u.Id == request.DocumentId);

        if (document == null)
        {
            throw new NotFoundException("Document not found.");
        }

        var fileExtension = Path.GetExtension(document.Path).ToLowerInvariant();
        
        var contentType = FileExtensionHelper.GetExtension(fileExtension);

        return new ReturnDocumentModel
        {
            Name = document.Name + fileExtension,
            Type = contentType,
            Bytes = await File.ReadAllBytesAsync(document.Path, cancellationToken)
        };
    }
}