using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations.Identity;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);

        builder.ToTable("UserRoles");

        builder.Property(x => x.UserId)
               .IsRequired();

        builder.Property(x => x.RoleId)
               .IsRequired();

        builder.Property(x => x.AssignedAt)
               .IsRequired();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.RoleId);

        builder.HasIndex(x => x.AssignedAt);
    }
}
