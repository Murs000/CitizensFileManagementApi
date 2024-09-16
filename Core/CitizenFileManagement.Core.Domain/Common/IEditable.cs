using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Core.Domain.Common;

public interface IEditable
{
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }
    public void SetModifyCredentials(int userId);
}