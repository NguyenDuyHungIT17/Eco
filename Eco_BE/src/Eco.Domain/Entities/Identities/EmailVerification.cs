using Eco.Domain.Common;
using Eco.Domain.Enum;

namespace Eco.Domain.Entities.Identities;

public class EmailVerification : BaseEntity
{
    public Guid UserId { get; set; }

    public string Token { get; set; } = default!;

    public Status.VerificationStatus Status { get; set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime? VerifiedAt { get; set; }
}