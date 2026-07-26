using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class LoginHistoryConfiguration : BaseEntityConfiguration<LoginHistory>
{
    public override void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable("LoginHistories");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.Browser)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.OperatingSystem)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.IpAddress)
               .HasMaxLength(45)
               .IsRequired();

        builder.Property(x => x.Location)
               .HasMaxLength(255);

        builder.Property(x => x.Success)
               .IsRequired();

        builder.Property(x => x.LoginAt)
               .IsRequired();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.LoginAt);

        builder.HasIndex(x => x.Success);

        builder.HasIndex(x => x.IpAddress);
    }
}