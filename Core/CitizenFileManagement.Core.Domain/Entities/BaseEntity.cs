using System;
using CitizenFileManagement.Core.Domain.Common;

namespace CitizenFileManagement.Core.Domain.Entities;

public class BaseEntity<T> : IEntity, IAuditable<T> where T : BaseEntity<T>
{
    public int Id { get; set; }

    public int? CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

    public T SetCreationCredentials(int userId)
    {
        CreatorId = userId;
        CreateDate = DateTime.UtcNow.AddHours(4);

        return (T)this;
    }

    public T SetCredentials(int userId)
    {
        if(CreateDate == null)
        {
            SetCreationCredentials(userId);
        }
        else
        {
            SetModifyCredentials(userId);
        }

        return (T)this;;
    }

    public T SetModifyCredentials(int userId)
    {
        ModifierId = userId;
        ModifyDate = DateTime.UtcNow.AddHours(4);

        return (T)this;;
    }
}
