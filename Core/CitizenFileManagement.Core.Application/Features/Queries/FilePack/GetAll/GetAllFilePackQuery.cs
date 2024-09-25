using CitizenFileManagement.Core.Application.Features.Queries.FilePack.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.FilePack.GetAll;

public class GetAllFilePackQuery : IRequest<List<FilePackViewModel>>
{
}