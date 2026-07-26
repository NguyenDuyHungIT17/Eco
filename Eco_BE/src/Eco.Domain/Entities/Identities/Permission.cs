using Eco.Domain.Common;

namespace Eco.Domain.Entities.Identities;

public class Permission : BaseEntity
{
    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; }
}