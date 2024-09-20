using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.Get;

public class GetDocumentQuery : IRequest<byte[]>
{
    public int DocumentId { get; set; }
}