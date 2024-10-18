using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class Document : BaseEntity<Document>
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string? Description { get; set; }
    public DocumentType Type { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int FilePackId { get; set; }
    public FilePack? FilePack { get; set; }

    public Document SetDetails(string name, string path, DocumentType type, string? description)
    {
        Name = name;
        Path = path;
        Description = description;
        Type = type;

        return this;
    }

    public Document SetUser(int userId)
    {
        UserId = userId;

        return this;
    }
    public Document SetFilePack(int packId)
    {
        FilePackId = packId;

        return this;
    }
}