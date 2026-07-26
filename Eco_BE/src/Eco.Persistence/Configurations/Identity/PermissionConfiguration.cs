using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class PermissionConfiguration : BaseEntityConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);

        builder.ToTable("Permissions");

        builder.Property(x => x.Code)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.HasIndex(x => x.Code)
               .IsUnique();

        builder.HasIndex(x => x.Name);
    }
}
