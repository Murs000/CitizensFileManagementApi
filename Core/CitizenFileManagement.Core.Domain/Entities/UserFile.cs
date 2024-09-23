using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class UserFile : IEntity, IAuditable<UserFile>
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Path { get; set; }

    public int FilePackId { get; set; }
    public FilePack FilePack { get; set; }

    public UserFile SetDetails(string name, string path)
    {
        Name = name;
        Path = path;

        return this;
    }

    public int? CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

    public UserFile SetCreationCredentials(int userId)
    {
        CreatorId = userId;
        CreateDate = DateTime.UtcNow.AddHours(4);

        return this;
    }

    public UserFile SetCredentials(int userId)
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

    public UserFile SetModifyCredentials(int userId)
    {
        ModifierId = userId;
        ModifyDate = DateTime.UtcNow.AddHours(4);

        return this;
    }
}