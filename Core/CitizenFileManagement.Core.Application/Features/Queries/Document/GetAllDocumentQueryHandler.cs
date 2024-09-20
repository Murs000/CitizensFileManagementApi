using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document;

public class GetAllDocumentQueryHandler : IRequestHandler<GetAllDocumentQuery, List<DocumentViewModel>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserManager _userManager;

    public GetAllDocumentQueryHandler(IDocumentRepository documentRepository, IUserManager userManager)
    {
        _documentRepository = documentRepository;
        _userManager = userManager;
    }

    public async Task<List<DocumentViewModel>> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var documents = await _documentRepository.GetAllAsync(u => u.CreatorId == userId);

        if (documents == null)
        {
            throw new NotFoundException("Documents not found.");
        }

        var documentViewModels = new List<DocumentViewModel>();

        foreach(var document in documents)
        {
            var documentViewModel = new DocumentViewModel
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                Type = document.Type
            };

            documentViewModels.Add(documentViewModel);
        }

        return documentViewModels;
    }
}