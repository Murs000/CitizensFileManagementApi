using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;

namespace CitizenFileManagement.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User> , IUserRepository
{
    public UserRepository(CitizenFileDB context) : base(context)
    {
    }
}