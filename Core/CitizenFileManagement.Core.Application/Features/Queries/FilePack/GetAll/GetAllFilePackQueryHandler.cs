using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.FilePack.ViewModels;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Core.Application.Features.Queries.Models;

namespace CitizenFileManagement.Core.Application.Features.Queries.FilePack.GetAll;

public class GetAllFilePackQueryHandler : IRequestHandler<GetAllFilePackQuery, List<FilePackViewModel>>
{
    private readonly IFilePackRepository _filePackRepository;
    private readonly IUserManager _userManager;

    public GetAllFilePackQueryHandler(IFilePackRepository filePackRepository, IUserManager userManager)
    {
        _filePackRepository = filePackRepository;
        _userManager = userManager;
    }

    public async Task<List<FilePackViewModel>> Handle(GetAllFilePackQuery request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var filePacks = await _filePackRepository.GetAllAsync(u => u.CreatorId == userId, "Files");

        // Filter the results
        filePacks = ApplyFilter(filePacks, request.FilterModel);

        // Search within the results
        filePacks = ApplySearch(filePacks, request.SearchTerm);

        // Apply pagination logic
        filePacks = ApplyPagination(filePacks, request.PaginationModel);

        if (filePacks == null)
        {
            throw new NotFoundException("FilePacks not found.");
        }

        var filePackViewModels = new List<FilePackViewModel>();

        foreach (var filePack in filePacks)
        {
            var filePackViewModel = new FilePackViewModel
            {
                Id = filePack.Id,
                Name = filePack.Name,
                Files = filePack.Files?.Select(fp => fp.Name).ToList() ?? []
            };

            filePackViewModels.Add(filePackViewModel);
        }

        return filePackViewModels;
    }
    // Filtration Logic
    private IEnumerable<Domain.Entities.FilePack> ApplyFilter(IEnumerable<Domain.Entities.FilePack> filePacks, FilterModel filter)
    {
        if (filter == null) return filePacks;

        // Example filter: Filter by CreatedDate, Status, or any other properties
        if (filter.CreatedAfter.HasValue)
        {
            filePacks = filePacks.Where(fp => fp.CreateDate >= filter.CreatedAfter.Value);
        }
        if (filter.CreatedBefore.HasValue)
        {
            filePacks = filePacks.Where(fp => fp.CreateDate <= filter.CreatedBefore.Value);
        }

        return filePacks;
    }

    // Search Logic
    private IEnumerable<Domain.Entities.FilePack> ApplySearch(IEnumerable<Domain.Entities.FilePack> filePacks, string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return filePacks;

        searchTerm = searchTerm.ToLower();
        filePacks = filePacks.Where(fp => fp.Name.ToLower().Contains(searchTerm));

        return filePacks;
    }

    // Pagination Logic
    private IEnumerable<Domain.Entities.FilePack> ApplyPagination(IEnumerable<Domain.Entities.FilePack> filePacks, PaginationModel paginationModel)
    {
        if (paginationModel.PageNumber <= 0) paginationModel.PageNumber = 1;
        if (paginationModel.PageSize <= 0) paginationModel.PageSize = 10;

        return filePacks.Skip((paginationModel.PageNumber - 1) * paginationModel.PageSize).Take(paginationModel.PageSize);
    }
}