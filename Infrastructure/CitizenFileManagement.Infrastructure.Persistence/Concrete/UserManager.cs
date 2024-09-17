using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.External.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class UserManager : IUserManager
{
    private readonly IClaimManager _claimManager;
    private readonly JwtSettings _jwtSettings;

    public UserManager(IClaimManager claimManager, IOptions<JwtSettings> jwtSettings)
    {
        _claimManager = claimManager;
        _jwtSettings = jwtSettings.Value;
    }

    public (string token, DateTime expireAt) GenerateTJwtToken(User user)
    {
        var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) };
        claims.AddRange(_claimManager.GetUserClaims(user));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expireAt = DateTime.UtcNow.AddMinutes(double.Parse(_jwtSettings.ExpireAt));
        var token = new JwtSecurityToken
            (claims: claims,
            expires: expireAt,
            signingCredentials: creadentials
            );
        var tokenHandler = new JwtSecurityTokenHandler();
        
        return (tokenHandler.WriteToken(token), expireAt);
    }

    public int GetCurrentUserId()
    {
        return _claimManager.GetCurrentUserId();
    }
}
