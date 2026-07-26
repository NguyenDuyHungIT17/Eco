using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class SubscriptionConfiguration : BaseEntityConfiguration<Subscription>
{
    public override void Configure(EntityTypeBuilder<Subscription> builder)
    {
        base.Configure(builder);

        builder.ToTable("Subscriptions");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.Plan)
               .IsRequired();

        builder.Property(x => x.Status)
               .IsRequired();

        builder.Property(x => x.AiCredits)
               .HasDefaultValue(0);

        builder.Property(x => x.StartedAt)
               .IsRequired();

        builder.Property(x => x.ExpiredAt)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .HasDefaultValue(false);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Plan);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.IsActive);

        builder.HasIndex(x => x.ExpiredAt);
    }
}
