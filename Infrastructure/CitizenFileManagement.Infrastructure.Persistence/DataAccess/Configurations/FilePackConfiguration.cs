using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Infrastructure.Persistence.DataAccess.Configurations;

public class FilePackConfiguration : IEntityTypeConfiguration<FilePack>
{
    public void Configure(EntityTypeBuilder<FilePack> builder)
    {
        builder.HasKey(fp => fp.Id);

        builder.Property(fp => fp.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(fp => fp.Description)
            .HasMaxLength(500);

        // Relationships
        builder.HasMany(fp => fp.FilePackDocuments)
            .WithOne(fpDoc => fpDoc.FilePack)
            .HasForeignKey(fpDoc => fpDoc.FilePackId)
            .OnDelete(DeleteBehavior.Cascade);  // Allow cascading delete for documents

        builder.HasOne(fp => fp.User)
            .WithMany(u => u.FilePacks)
            .HasForeignKey(fp => fp.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

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