using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Application.Features.Queries.Models;

public class ReturnDocumentModel
{
    public string Name { get; set; }
    public string Type { get; set; }
    public byte[] Bytes { get; set; }
}