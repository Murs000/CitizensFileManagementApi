using CitizenFileManagement.Core.Domain.Entities;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CitizenFileManagement.Infrastructure.Persistence.DataAccess;

public class CitizenFileDB : DbContext
{
    public CitizenFileDB(DbContextOptions<CitizenFileDB> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<FilePack> FilePacks { get; set; }
    public DbSet<FilePackDocument> FilePackDocuments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new FilePackConfiguration());
        modelBuilder.ApplyConfiguration(new FilePackDocumentConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}

