using CitizenFileManagement.Core.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;

public class DocumentDTO
{
    public string Name { get; set; }
    public IFormFile File { get; set; }
    public string? Description { get; set; }
    public DocumentType Type { get; set; }
}