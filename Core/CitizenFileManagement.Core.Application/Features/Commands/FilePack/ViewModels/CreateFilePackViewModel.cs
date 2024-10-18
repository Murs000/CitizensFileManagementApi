using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.ViewModels;

public class CreateFilePackViewModel : IRequest<bool>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFileCollection? Files { get; set; }
}