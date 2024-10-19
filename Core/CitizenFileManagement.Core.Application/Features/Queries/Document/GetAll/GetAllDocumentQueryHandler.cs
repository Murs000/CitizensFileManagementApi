using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Common.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Models;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetAll;

public class GetAllDocumentQueryHandler : IRequestHandler<GetAllDocumentQuery, ReturnItemModel<DocumentViewModel>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IUserManager _userManager;

    public GetAllDocumentQueryHandler(IDocumentRepository documentRepository, IUserManager userManager)
    {
        _documentRepository = documentRepository;
        _userManager = userManager;
    }

    public async Task<ReturnItemModel<DocumentViewModel>> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var documents = await _documentRepository.GetAllAsync(u => u.CreatorId == userId);

        var defaultPackId = documents.Select(d => d.Id).Min();
        documents = documents.Where(d => d.FilePackId == defaultPackId);

        // Filter the results
        documents = ApplyFilter(documents, request.FilterModel);

        int count = documents.Count();

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

        return new ReturnItemModel<DocumentViewModel>
        {
            Items = documentViewModels,
            Count = count
        };
    }
    // Filtration Logic
    private IEnumerable<Domain.Entities.Document> ApplyFilter(IEnumerable<Domain.Entities.Document> documents, FilterModel? filter)
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

        if (string.IsNullOrEmpty(filter.SearchTerm)) return documents;

        filter.SearchTerm = filter.SearchTerm.ToLower();
        documents = documents.Where(fp => fp.Name.ToLower().Contains(filter.SearchTerm) || 
                                    (fp.Description != null && fp.Description.ToLower().Contains(filter.SearchTerm)));

        return documents;
    }

    // Pagination Logic
    private IEnumerable<Domain.Entities.Document> ApplyPagination(IEnumerable<Domain.Entities.Document> documents, PaginationModel? paginationModel)
    {
        if (paginationModel == null || (paginationModel.PageSize == 0 && paginationModel.PageNumber == 0)) return documents;

        return documents.Skip((paginationModel.PageNumber - 1) * paginationModel.PageSize).Take(paginationModel.PageSize);
    }
}