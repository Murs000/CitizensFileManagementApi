using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class Document : IEntity, IAuditable<Document>
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Path { get; set; }
    public string? Description { get; set; }
    public DocumentType Type { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public Document SetDetails(string name, string path, string description, DocumentType type)
    {
        Name = name;
        Path = path;
        Description = description;
        Type = type;

        return this;
    }

    public int? CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

    public Document SetCreationCredentials(int userId)
    {
        CreatorId = userId;
        CreateDate = DateTime.UtcNow.AddHours(4);

        return this;
    }

    public Document SetCredentials(int userId)
    {
        if(CreateDate == null)
        {
            SetCreationCredentials(userId);
        }
        else
        {
            SetModifyCredentials(userId);
        }
        return this;
    }

    public Document SetModifyCredentials(int userId)
    {
        ModifierId = userId;
        ModifyDate = DateTime.UtcNow.AddHours(4);

        return this;
    }
}