using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;

public class GetMultipleDocumentQuery : IRequest<ReturnDocumentViewModel>
{
    public List<int> DocumentIds { get; set; }
}