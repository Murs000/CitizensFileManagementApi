using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.GetMultiple;

public class GetByTypeDocumentQuery : IRequest<ReturnDocumentModel>
{
    public DocumentType Type { get; set; }
}