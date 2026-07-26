using Eco.Domain.Common;
using static Eco.Domain.Enum.Otp;

namespace Eco.Domain.Entities.Identities;

public class Otp : BaseEntity
{
    public Guid UserId { get; set; }

    public string Code { get; set; } = default!;

    public OtpPurpose Purpose { get; set; } = default!;

    public DateTime ExpiredAt { get; set; }

    public DateTime? VerifiedAt { get; set; }
}