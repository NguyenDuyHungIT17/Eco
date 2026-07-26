using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class AuditLogConfiguration : BaseEntityConfiguration<AuditLog>
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        base.Configure(builder);

        builder.ToTable("AuditLogs");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.Action)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.Property(x => x.IpAddress)
               .HasMaxLength(45);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Action);

        builder.HasIndex(x => x.CreatedAt);
    }
}