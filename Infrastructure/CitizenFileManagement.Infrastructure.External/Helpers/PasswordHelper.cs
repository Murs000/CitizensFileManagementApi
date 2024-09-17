using System.Security.Cryptography;
using System.Text;

namespace CitizenFileManagement.Infrastructure.External.Helpers;

public static class PasswordHelper
{
    public static (string passwordHash, string passwordSalt) HashPassword(string password)
    {
        using var hmac = new HMACSHA256();
        var salt = hmac.Key;
        var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        var passwordSalt = Convert.ToBase64String(salt);
        return (passwordHash, passwordSalt);
    }
    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        using var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(computedHash) == storedHash;
    }
}