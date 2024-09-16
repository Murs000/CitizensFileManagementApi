using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;

namespace CitizenFileManagement.Infrastructure.Persistence.Repositories;

public class DocumentRepository : Repository<Document> , IDocumentRepository
{
    public DocumentRepository(CitizenFileDB context) : base(context)
    {
    }
}