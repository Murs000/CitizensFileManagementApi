
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;

namespace CitizenFileManagement.Infrastructure.Persistence.Repositories;

public class FilePackRepository : Repository<FilePack> , IFilePackRepository
{
    public FilePackRepository(CitizenFileDB context) : base(context)
    {
    }
}