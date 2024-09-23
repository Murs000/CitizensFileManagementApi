using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class FilePack : IEntity, IAuditable<FilePack>
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public List<string> Paths { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public FilePack SetDetails(string name, string description)
    {
        Name = name;
        Description = description;

        return this;
    }

    public int? CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

    public FilePack SetCreationCredentials(int userId)
    {
        CreatorId = userId;
        CreateDate = DateTime.UtcNow.AddHours(4);

        return this;
    }

    public FilePack SetCredentials(int userId)
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

    public FilePack SetModifyCredentials(int userId)
    {
        ModifierId = userId;
        ModifyDate = DateTime.UtcNow.AddHours(4);

        return this;
    }
}