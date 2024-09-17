using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Core.Application.Interfaces;

public interface IUserManager
{
    public int GetCurrentUserId();
    (string token,DateTime expireAt) GenerateTJwtToken(User user);
}
