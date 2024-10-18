using CitizenFileManagement.Core.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CitizenFileManagement.Core.Application.Features.Commands.Document.ViewModels;

public class UpdateDocumentDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IFormFile? File { get; set; }
    public string? Description { get; set; }
    public DocumentType Type { get; set; }
}