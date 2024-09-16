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
            .IsRequired();

        builder.Property(d => d.CreateDate).IsRequired(false);
        builder.Property(d => d.ModifyDate).IsRequired(false);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);
    }
}