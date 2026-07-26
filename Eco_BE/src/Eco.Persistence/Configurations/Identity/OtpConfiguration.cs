using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class OtpConfiguration : BaseEntityConfiguration<Otp>
{
    public override void Configure(EntityTypeBuilder<Otp> builder)
    {
        base.Configure(builder);

        builder.ToTable("Otps");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.Code)
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(x => x.Purpose)
               .IsRequired();

        builder.Property(x => x.ExpiredAt)
               .IsRequired();

        builder.Property(x => x.VerifiedAt);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Code);

        builder.HasIndex(x => x.Purpose);

        builder.HasIndex(x => x.ExpiredAt);
    }
}