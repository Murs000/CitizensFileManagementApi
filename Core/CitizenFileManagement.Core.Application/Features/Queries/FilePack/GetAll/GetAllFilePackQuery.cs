using CitizenFileManagement.Core.Application.Features.Queries.FilePack.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;
using CitizenFileManagement.Core.Application.Features.Queries.Models;

namespace CitizenFileManagement.Core.Application.Features.Queries.FilePack.GetAll;

public class GetAllFilePackQuery : IRequest<List<FilePackViewModel>>
{
    public string? SearchTerm { get; set; }
    public FilterModel? FilterModel { get; set; }
    public PaginationModel? PaginationModel { get; set; }

}