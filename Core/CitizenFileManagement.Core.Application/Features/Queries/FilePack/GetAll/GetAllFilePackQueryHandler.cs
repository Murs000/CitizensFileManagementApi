using MediatR;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.FilePack.ViewModels;
using CitizenFileManagement.Core.Domain.Entities;

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

        if (filePacks == null)
        {
            throw new NotFoundException("FilePacks not found.");
        }

        var filePackViewModels = new List<FilePackViewModel>();

        foreach(var filePack in filePacks)
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
}