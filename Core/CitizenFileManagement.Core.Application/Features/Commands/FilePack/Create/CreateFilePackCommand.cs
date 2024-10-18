using CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Create;

public class CreateFilePackCommand : IRequest<bool>
{
    public List<CreateFilePackViewModel> FilePacks { get; set; }
}
 