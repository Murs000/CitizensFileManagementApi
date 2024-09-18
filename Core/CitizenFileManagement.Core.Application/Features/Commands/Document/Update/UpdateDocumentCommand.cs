using CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Update;

public class UpdateDocumentCommand : IRequest<bool>
{
    public List<int> DeletedFiles { get; set; }
    public List<DocumentDTO> Files { get; set; }
}