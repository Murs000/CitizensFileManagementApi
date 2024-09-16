using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Core.Domain.Common;

public interface ICreatable
{
    public int? CreaterId { get; set; }
    public User? Creater { get; set; } 
    public DateTime? CreateDate { get; set; }

    public void SetCreationCredentials(int userId);
}