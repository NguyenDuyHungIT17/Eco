using Eco.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eco.Persistence.Configurations;

public abstract class BaseEntityConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        // Id
        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        // Audit
        builder.Property(x => x.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.CreatedBy);

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.UpdatedBy);

        builder.Property(x => x.IsDeleted)
               .IsRequired()
               .HasDefaultValue(false);
    }
}