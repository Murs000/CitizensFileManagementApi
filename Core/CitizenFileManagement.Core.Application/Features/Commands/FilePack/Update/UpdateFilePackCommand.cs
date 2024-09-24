using CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;
using CitizenFileManagement.Core.Application.Features.Commands.FilePack.ViewModels;
using CitizenFileManagement.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.Update;

public class UpdateFilePackCommand : IRequest<bool>
{
    public List<int> FileIds { get; set; }

    public List<FilePackViewModel> Files { get; set; }
}