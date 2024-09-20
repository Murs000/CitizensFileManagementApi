using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Application.Features.Queries.Document.ViewModels;

public class ReturnDocumentViewModel
{
    public string Name { get; set; }
    public string Type { get; set; }
    public byte[] Bytes { get; set; }
}