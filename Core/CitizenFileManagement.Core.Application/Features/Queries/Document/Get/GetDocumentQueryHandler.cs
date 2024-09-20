using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.Get;

public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, byte[]>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserManager _userManager;

    public GetDocumentQueryHandler(IDocumentRepository documentRepository, IUserManager userManager)
    {
        _documentRepository = documentRepository;
        _userManager = userManager;
    }

    public async Task<byte[]> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var document = await _documentRepository.GetAsync(u => u.CreatorId == userId && u.Id == request.DocumentId);

        if (document == null)
        {
            throw new NotFoundException("Document not found.");
        }

        var documentViewModels = new List<DocumentViewModel>();

        return await File.ReadAllBytesAsync(document.Path, cancellationToken);
    }
}