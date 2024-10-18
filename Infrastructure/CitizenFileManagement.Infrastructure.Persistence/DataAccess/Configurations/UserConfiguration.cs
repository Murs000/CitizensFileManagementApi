using CitizenFileManagement.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .IsUnique();

        // Email - required, max length 255, and unique
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        // Name and Surname - required
        builder.Property(u => u.Name)
            .IsRequired();
        
        builder.Property(u => u.Surname)
            .IsRequired();

        // PasswordHash and PasswordSalt - required
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.Property(u => u.PasswordSalt)
            .IsRequired();

        // Role - required, enum-based conversion
        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>();  // Storing as string

        // Relationships
        builder.HasMany(u => u.Documents)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete
        
        builder.HasMany(u => u.FilePacks)
            .WithOne(fp => fp.User)
            .HasForeignKey(fp => fp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Creator)
            .WithMany()
            .HasForeignKey(u => u.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Modifier)
            .WithMany()
            .HasForeignKey(u => u.ModifierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Audit fields
        builder.Property(u => u.CreateDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.ModifyDate)
            .IsRequired(false);

        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Soft-delete filter
        builder.HasQueryFilter(u => u.IsDeleted == false);
    }
}