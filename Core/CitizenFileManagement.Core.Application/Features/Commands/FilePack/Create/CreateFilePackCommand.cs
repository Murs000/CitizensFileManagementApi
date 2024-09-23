using CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

public class CreateFilePackCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFileCollection Files { get; set; }
}
 