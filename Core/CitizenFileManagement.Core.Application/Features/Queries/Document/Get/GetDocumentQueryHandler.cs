using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.Get;

public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, ReturnDocumentViewModel>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserManager _userManager;

    public GetDocumentQueryHandler(IDocumentRepository documentRepository, IUserManager userManager)
    {
        _documentRepository = documentRepository;
        _userManager = userManager;
    }

    public async Task<ReturnDocumentViewModel> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var document = await _documentRepository.GetAsync(u => u.CreatorId == userId && u.Id == request.DocumentId);

        if (document == null)
        {
            throw new NotFoundException("Document not found.");
        }

        var fileExtension = Path.GetExtension(document.Path).ToLowerInvariant();

        var contentType = fileExtension switch
        {
            ".pdf" => "application/pdf",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".jpg" => "image/jpeg",
            ".png" => "image/png",
            // Add more file types as needed
            _ => "application/octet-stream"
        };

        return new ReturnDocumentViewModel
        {
            Name = document.Name + fileExtension,
            Type = contentType,
            Bytes = await File.ReadAllBytesAsync(document.Path, cancellationToken)
        };
    }
}