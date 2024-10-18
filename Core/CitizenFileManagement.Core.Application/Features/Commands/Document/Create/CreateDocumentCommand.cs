using CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Create;

public class CreateDocumentCommand : IRequest<bool>
{
    public List<CreateDocumentDTO> Files { get; set; }
}
 