using CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.Delete;

public class DeleteDocumentCommand : IRequest<bool>
{
    public List<int> DeletedFiles { get; set; }
}
 