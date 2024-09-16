using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class User : IEntity, IAuditable
{
    public int Id { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PaswordSalt { get; set; }
    public UserRole Role { get; set; }

    public void SetPassword(string passwordHash, string paswordSalt)
    {
        PasswordHash = passwordHash;
        PaswordSalt = paswordSalt;
    }
    public void SetDetails(string username, string email, UserRole role)
    {
        Email = email;
        Username = username;
        Role = role;
    }


    public int? CreaterId { get; set; }
    public User? Creater { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

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