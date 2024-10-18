using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class User : BaseEntity<User>
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireDate { get; set; }
    public string? OTP { get; set; }
    public DateTime? OTPExpireDate { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public bool IsActivated { get; set; }
    public UserRole Role { get; set; }

    public ICollection<Document> Documents { get; set; }
    public ICollection<FilePack> FilePacks { get; set; }


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
    public User SetDetails(string username, string name, string surname, string email, UserRole role)
    {
        Email = email;
        Username = username;
        Name = name;
        Surname = surname;
        Role = role;

        return this;
    }
    public User UpdateDetails(string name, string surname, UserRole role)
    {
        Name = name;
        Surname = surname;
        Role = role;

        return this;
    }
}