using Eco.Domain.Common;

namespace Eco.Domain.Entities.Identities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }

    public string Token { get; set; } = default!;

    public DateTime ExpiredAt { get; set; }

    public DateTime RevokedAt { get; set; }

    public string DeviceId { get; set; }

    public bool IsRevoked { get; set; }
}