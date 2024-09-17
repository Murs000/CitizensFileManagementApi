using CitizenFileManagement.Core.Domain.Common;

namespace CitizenFileManagement.Core.Domain.Entities;

public class Customer : IEntity, IAuditable<Customer>
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public List<Document>? Documents { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public Customer SetDetails(string name, string surname)
    {
        Name = name;
        Surname = surname;

        return this;
    }

    public int? CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

    public Customer SetCreationCredentials(int userId)
    {
        CreatorId = userId;
        CreateDate = DateTime.UtcNow.AddHours(4);

        return this;
    }

    public Customer SetCredentials(int userId)
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

    public Customer SetModifyCredentials(int userId)
    {
        ModifierId = userId;
        ModifyDate = DateTime.UtcNow.AddHours(4);

        return this;
    }
}