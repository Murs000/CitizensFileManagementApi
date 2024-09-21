using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;

public class GetByTypeDocumentQuery : IRequest<ReturnDocumentViewModel>
{
    public DocumentType Type { get; set; }
}