using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityRoleMapping : IEntityTypeConfiguration<IdentityRole<long>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<long>> builder)
    {
        builder.ToTable("IdentityRole");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.NormalizedName).IsUnique();
        builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.NormalizedName).HasMaxLength(256);
    }
}