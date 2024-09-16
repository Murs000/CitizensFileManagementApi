using CitizenFileManagement.Core.Domain.Common;

namespace CitizenFileManagement.Core.Domain.Entities;

public class Customer : IEntity, IAuditable
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public List<Document> Documents { get; set; }

    public void SetDetails(string name, string surname)
    {
        Name = name;
        Surname = surname;
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