using CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Queries.Models;
using CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;
using MediatR;

namespace CitizenFileManagement.Core.Application.Features.Queries.FilePack.Get;

public class GetFilePackQuery : IRequest<ReturnDocumentModel>
{
    public int FilePackId { get; set; }
}