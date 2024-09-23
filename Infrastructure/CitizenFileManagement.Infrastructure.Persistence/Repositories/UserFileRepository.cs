using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;

namespace CitizenFileManagement.Infrastructure.Persistence.Repositories;

public class UserFileRepository : Repository<UserFile> , IUserFileRepository
{
    public UserFileRepository(CitizenFileDB context) : base(context)
    {
    }
}