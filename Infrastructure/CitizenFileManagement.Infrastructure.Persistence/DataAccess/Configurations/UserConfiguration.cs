using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Infrastructure.Persistence.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        // Username - required, max length 150, and unique
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.Username)
            .IsUnique(); // Enforce uniqueness on Username

        // Email - required, max length 255, and unique
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique(); // Enforce uniqueness on Email

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.PasswordSalt)
            .IsRequired();

        builder.Property(u => u.Role)
            .IsRequired();

        // Relationship with Customer
        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId);

        // Audit fields
        builder.Property(u => u.CreateDate).IsRequired(false);
        builder.Property(u => u.ModifyDate).IsRequired(false);
        builder.Property(u => u.IsDeleted).HasDefaultValue(false);
    }
}