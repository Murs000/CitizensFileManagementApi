using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Core.Domain.Common;

public interface ICreatable<T>
{
    public int? CreatorId { get; set; }
    public User? Creator { get; set; } 
    public DateTime? CreateDate { get; set; }

    public T SetCreationCredentials(int? userId);
}