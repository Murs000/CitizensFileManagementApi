using System.Security.Claims;
using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Core.Application.Interfaces;

public interface IClaimManager
{
    int GetCurrentUserId();
    IEnumerable<Claim> GetUserClaims(User user);
}
