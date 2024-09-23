using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Infrastructure.Persistence.DataAccess.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);  // Set primary key

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100); // Configure Name column

        builder.Property(c => c.Surname)
            .IsRequired()
            .HasMaxLength(100); // Configure Surname column

        // Configure relationships
        builder.HasOne(c => c.User)
            .WithOne(u => u.Customer)
            .HasForeignKey<User>(u => u.CustomerId);

        builder.HasMany(c => c.Documents)
            .WithOne()
            .HasForeignKey(d => d.CustomerId); // Assuming Document has a foreign key to Customer

        builder.HasMany(c => c.FilePacks)
            .WithOne()
            .HasForeignKey(d => d.CustomerId);

        builder.HasOne(c => c.Creator)
            .WithMany()
            .HasForeignKey(c => c.CreatorId);

        builder.HasOne(c => c.Modifier)
            .WithMany()
            .HasForeignKey(c => c.ModifierId);

        // Audit fields (optional)
        builder.Property(c => c.CreateDate).IsRequired(false);
        builder.Property(c => c.ModifyDate).IsRequired(false);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(d => d.IsDeleted == false);
    }
}