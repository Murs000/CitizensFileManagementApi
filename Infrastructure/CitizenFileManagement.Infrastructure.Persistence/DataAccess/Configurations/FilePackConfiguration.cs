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

        builder.HasMany(fp => fp.Files)
            .WithOne()
            .HasForeignKey(uf => uf.FilePackId);

        builder.HasOne(fp => fp.Customer)
            .WithMany(c => c.FilePacks)
            .HasForeignKey(fp => fp.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fp => fp.Creator)
            .WithMany()
            .HasForeignKey(fp => fp.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fp => fp.Modifier)
            .WithMany()
            .HasForeignKey(fp => fp.ModifierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Optional: Configure auditing fields as required
        builder.Property(fp => fp.CreateDate).IsRequired(false);
        builder.Property(fp => fp.ModifyDate).IsRequired(false);
        builder.Property(fp => fp.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(fp => fp.IsDeleted == false);
    }
}