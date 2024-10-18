using System;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;

namespace CitizenFileManagement.Infrastructure.Persistence.Repositories;

public class FilePackDocumentRepository : Repository<FilePackDocument> , IFilePackDocumentRepository
{
    public FilePackDocumentRepository(CitizenFileDB context) : base(context)
    {
    }
}
