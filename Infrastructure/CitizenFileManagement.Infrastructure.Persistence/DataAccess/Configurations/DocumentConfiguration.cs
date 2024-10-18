using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Infrastructure.Persistence.DataAccess.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.Path)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.Type)
            .IsRequired()
            .HasConversion<string>();

        // Relationships
        builder.HasOne(d => d.User)
            .WithMany(u => u.Documents)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

        // Relationships
        builder.HasOne(d => d.FilePack)
            .WithMany(u => u.Documents)
            .HasForeignKey(d => d.FilePackId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);  // Prevent cascading delete

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
            .IsRequired();

        builder.Property(u => u.ModifyDate)
            .IsRequired(false);

        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Soft-delete filter
        builder.HasQueryFilter(u => u.IsDeleted == false);
    }
}