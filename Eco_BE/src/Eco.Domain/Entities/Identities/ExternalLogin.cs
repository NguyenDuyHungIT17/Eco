using Eco.Domain.Common;
using Eco.Domain.Enum;

namespace Eco.Domain.Entities.Identities;

public class ExternalLogin : BaseEntity
{
    public Guid UserId { get; set; }

    public LoginProvider Provider { get; set; } = default!;

    public string ProviderUserId { get; set; } = default!;

    public string ProviderEmail { get; set; }
}