using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class Document : IEntity, IAuditable
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Path { get; set; }
    public string Description { get; set; }
    public DocumentType Type { get; set; }
    public void SetDetails(string name, string path, string description, DocumentType type)
    {
        Name = name;
        Path = path;
        Description = description;
        Type = type;
    }

    public int? CreaterId { get; set; }
    public User? Creater { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }

    public void SetCreationCredentials(int userId)
    {
        CreaterId = userId;
        CreateDate = DateTime.UtcNow.AddHours(4);
    }

    public void SetCredentials(int userId)
    {
        if(CreateDate == null)
        {
            SetCreationCredentials(userId);
        }
        else
        {
            SetModifyCredentials(userId);
        }
    }

    public void SetModifyCredentials(int userId)
    {
        ModifierId = userId;
        ModifyDate = DateTime.UtcNow.AddHours(4);
    }
}