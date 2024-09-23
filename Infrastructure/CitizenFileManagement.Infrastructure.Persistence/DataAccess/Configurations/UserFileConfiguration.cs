using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CitizenFileManagement.Core.Domain.Entities;

namespace CitizenFileManagement.Infrastructure.Persistence.DataAccess.Configurations;

public class UserFileConfiguration : IEntityTypeConfiguration<UserFile>
{
    public void Configure(EntityTypeBuilder<UserFile> builder)
    {
        builder.HasKey(fp => fp.Id);

        builder.Property(fp => fp.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(uf => uf.FilePack)
            .WithMany(fp => fp.Files)
            .HasForeignKey(fp => fp.FilePackId)
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