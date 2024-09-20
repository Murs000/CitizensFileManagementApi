using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document;

public class GetAllDocumentQuery : IRequest<List<DocumentViewModel>>
{
}