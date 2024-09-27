using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetAll;

public class GetAllDocumentQuery : IRequest<List<DocumentViewModel>>
{
    public string? SearchTerm { get; set; }
    public FilterModel? FilterModel { get; set; }
    public PaginationModel? PaginationModel { get; set; }
}