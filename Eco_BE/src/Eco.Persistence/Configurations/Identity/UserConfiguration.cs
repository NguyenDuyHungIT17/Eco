using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.Username)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Email)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(x => x.PasswordHash)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(x => x.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(x => x.EmailVerified)
               .HasDefaultValue(false);

        builder.Property(x => x.PhoneVerified)
               .HasDefaultValue(false);

        builder.Property(x => x.IsLocked)
               .HasDefaultValue(false);

        builder.Property(x => x.FailedLoginCount)
               .HasDefaultValue(0);

        builder.Property(x => x.LastLoginAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => x.Username)
               .IsUnique();

        builder.HasIndex(x => x.Email)
               .IsUnique();

        builder.HasIndex(x => x.PhoneNumber);

        builder.Property(x => x.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.IsDeleted)
               .HasDefaultValue(false);
    }
}