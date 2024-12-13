using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityUserLoginMapping : IEntityTypeConfiguration<IdentityUserLogin<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<long>> builder)
    {
        builder.ToTable("IdentityUserLogin");
        builder.HasKey(x => new { x.LoginProvider, x.ProviderKey });
        builder.Property(x => x.LoginProvider).HasMaxLength(128).IsRequired();
        builder.Property(x => x.ProviderKey).HasMaxLength(128).IsRequired();
        builder.Property(x => x.ProviderDisplayName).HasMaxLength(128);
    }
}