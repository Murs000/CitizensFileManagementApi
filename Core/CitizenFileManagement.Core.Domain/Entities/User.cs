using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class User : IEntity, IAuditable<User>
{
    public int Id { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireDate { get; set; }
    public string? OTP { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public bool IsActivated { get; set; }
    public UserRole Role { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public User SetRefreshToken(string refreshToken, DateTime refreshTokenExpireDate)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpireDate = refreshTokenExpireDate;

        return this;
    } 

    public User SetPassword(string passwordHash, string paswordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = paswordSalt;

        return this;
    }
    public User SetDetails(string username, string email, UserRole role)
    {
        Email = email;
        Username = username;
        Role = role;

        return this;
    }


    public int? CreatorId { get; set; }
    public User? Creator { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? ModifierId { get; set; }
    public User? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

    public User SetCreationCredentials(int userId)
    {
        CreatorId = userId;
        CreateDate = DateTime.UtcNow.AddHours(4);

        return this;
    }

    public User SetCredentials(int userId)
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

    public User SetModifyCredentials(int userId)
    {
        ModifierId = userId;
        ModifyDate = DateTime.UtcNow.AddHours(4);

        return this;
    }
}