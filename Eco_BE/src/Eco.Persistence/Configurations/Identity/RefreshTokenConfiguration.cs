using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("RefreshTokens");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.Token)
               .HasMaxLength(1000)
               .IsRequired();

        builder.Property(x => x.ExpiredAt)
               .IsRequired();

        builder.Property(x => x.RevokedAt);

        builder.Property(x => x.DeviceId)
               .HasMaxLength(255);

        builder.Property(x => x.IsRevoked)
               .HasDefaultValue(false);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Token)
               .IsUnique();

        builder.HasIndex(x => x.ExpiredAt);

        builder.HasIndex(x => x.IsRevoked);
    }
}
