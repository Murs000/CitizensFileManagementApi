using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Models;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetAll;

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

        // Filter the results
        documents = ApplyFilter(documents, request.FilterModel);

        // Search within the results
        documents = ApplySearch(documents, request.SearchTerm);

        // Apply pagination logic
        documents = ApplyPagination(documents, request.PaginationModel);

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
    // Filtration Logic
    private IEnumerable<Domain.Entities.Document> ApplyFilter(IEnumerable<Domain.Entities.Document> documents, FilterModel filter)
    {
        if (filter == null) return documents;

        // Example filter: Filter by CreatedDate, Status, or any other properties
        if (filter.CreatedAfter.HasValue)
        {
            documents = documents.Where(fp => fp.CreateDate >= filter.CreatedAfter.Value);
        }
        if (filter.CreatedBefore.HasValue)
        {
            documents = documents.Where(fp => fp.CreateDate <= filter.CreatedBefore.Value);
        }

        return documents;
    }

    // Search Logic
    private IEnumerable<Domain.Entities.Document> ApplySearch(IEnumerable<Domain.Entities.Document> documents, string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return documents;

        searchTerm = searchTerm.ToLower();
        documents = documents.Where(fp => fp.Name.ToLower().Contains(searchTerm));

        return documents;
    }

    // Pagination Logic
    private IEnumerable<Domain.Entities.Document> ApplyPagination(IEnumerable<Domain.Entities.Document> documents, PaginationModel paginationModel)
    {
        if (paginationModel.PageNumber <= 0) paginationModel.PageNumber = 1;
        if (paginationModel.PageSize <= 0) paginationModel.PageSize = 10;

        return documents.Skip((paginationModel.PageNumber - 1) * paginationModel.PageSize).Take(paginationModel.PageSize);
    }
}