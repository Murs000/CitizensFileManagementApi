using System.Security.Cryptography;
using System.Text;

namespace CitizenFileManagement.Infrastructure.External.Helpers;

public static class RefreshTokenHelper
{
    public static string GenerateRefreshToken(int userId)
    {
        var random = GenerateRandomNumber();
        var refreshToken = $"{random}_{userId}_{DateTime.UtcNow.AddDays(20)}";
        return refreshToken;
    }

    private static object GenerateRandomNumber()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}