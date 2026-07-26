using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class PasswordResetConfiguration : BaseEntityConfiguration<PasswordReset>
{
    public override void Configure(EntityTypeBuilder<PasswordReset> builder)
    {
        base.Configure(builder);

        builder.ToTable("PasswordResets");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.Token)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(x => x.Status)
               .IsRequired();

        builder.Property(x => x.ExpiredAt)
               .IsRequired();

        builder.Property(x => x.UsedAt);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Token)
               .IsUnique();

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.ExpiredAt);
    }
}
