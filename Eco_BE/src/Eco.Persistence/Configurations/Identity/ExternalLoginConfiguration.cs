using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class ExternalLoginConfiguration : BaseEntityConfiguration<ExternalLogin>
{
    public override void Configure(EntityTypeBuilder<ExternalLogin> builder)
    {
        base.Configure(builder);

        builder.ToTable("ExternalLogins");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.Provider)
               .IsRequired();

        builder.Property(x => x.ProviderUserId)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(x => x.ProviderEmail)
               .HasMaxLength(255);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => new { x.Provider, x.ProviderUserId })
               .IsUnique();

        builder.HasIndex(x => x.ProviderEmail);
    }
}