using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Application.Features.Queries.User.ViewModels;

public class DocumentViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DocumentType Type { get; set; }
}