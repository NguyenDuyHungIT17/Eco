using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class UserProfileConfiguration : BaseEntityConfiguration<UserProfile>
{
    public override void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserProfiles");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.DisplayName)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.FirstName)
               .HasMaxLength(100);

        builder.Property(x => x.LastName)
               .HasMaxLength(100);

        builder.Property(x => x.Avatar)
               .HasMaxLength(1000);

        builder.Property(x => x.Gender);

        builder.Property(x => x.Birthday);

        builder.Property(x => x.Country)
               .HasMaxLength(100);

        builder.Property(x => x.Timezone)
               .HasMaxLength(100);

        builder.Property(x => x.Language)
               .HasMaxLength(50);

        builder.Property(x => x.Bio)
               .HasMaxLength(2000);

        builder.HasIndex(x => x.UserId)
               .IsUnique();

        builder.HasIndex(x => x.DisplayName);
    }
}
