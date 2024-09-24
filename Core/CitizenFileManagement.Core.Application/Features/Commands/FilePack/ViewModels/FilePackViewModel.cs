using MediatR;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.FilePack.ViewModels;

public class FilePackViewModel : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFileCollection Files { get; set; }
}