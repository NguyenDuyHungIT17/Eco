using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
{
    public override void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        base.Configure(builder);

        builder.ToTable("RolePermissions");

        builder.Property(x => x.RoleId)
               .IsRequired();

        builder.Property(x => x.PermissionId)
               .IsRequired();

        builder.HasIndex(x => x.RoleId);

        builder.HasIndex(x => x.PermissionId);
    }
}
