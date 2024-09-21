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

        builder.HasOne(d => d.Creator)
            .WithMany()
            .HasForeignKey(d => d.CreatorId);

        builder.HasOne(d => d.Modifier)
            .WithMany()
            .HasForeignKey(d => d.ModifierId);

        builder.HasOne(c => c.Customer)
            .WithMany()
            .HasForeignKey(c => c.CustomerId);

        builder.Property(d => d.CreateDate).IsRequired(false);
        builder.Property(d => d.ModifyDate).IsRequired(false);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(d => d.IsDeleted == false);
    }
}