using CitizenFileManagement.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizenFileManagement.Infrastructure.Persistence.DataAccess.Configurations;

public class FilePackDocumentConfiguration : IEntityTypeConfiguration<FilePackDocument>
{
    public void Configure(EntityTypeBuilder<FilePackDocument> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(fpDoc => fpDoc.AddedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(fpDoc => fpDoc.FilePack)
            .WithMany(fp => fp.FilePackDocuments)
            .HasForeignKey(fpDoc => fpDoc.FilePackId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fpDoc => fpDoc.Document)
            .WithMany(d => d.FilePackDocuments)
            .HasForeignKey(fpDoc => fpDoc.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

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