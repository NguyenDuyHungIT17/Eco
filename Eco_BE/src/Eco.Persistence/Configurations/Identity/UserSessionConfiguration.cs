using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class UserSessionConfiguration : BaseEntityConfiguration<UserSession>
{
    public override void Configure(EntityTypeBuilder<UserSession> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserSessions");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.RefreshTokenId)
               .IsRequired();

        builder.Property(x => x.DeviceId)
               .HasMaxLength(255);

        builder.Property(x => x.Browser)
               .HasMaxLength(200);

        builder.Property(x => x.OperatingSystem)
               .HasMaxLength(200);

        builder.Property(x => x.IpAddress)
               .HasMaxLength(45);

        builder.Property(x => x.Location)
               .HasMaxLength(500);

        builder.Property(x => x.LastActive)
               .IsRequired();

        builder.Property(x => x.ExpiredAt)
               .IsRequired();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.RefreshTokenId);

        builder.HasIndex(x => x.LastActive);

        builder.HasIndex(x => x.ExpiredAt);
    }
}
