using CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.AddDocument;

public class AddDocumentFilePackCommand : IRequest<bool>
{
    public List<int> DocumentIds { get; set; }
    public int PackId { get; set;}
}
 