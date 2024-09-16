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
    public DbSet<Customer> Customers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}

