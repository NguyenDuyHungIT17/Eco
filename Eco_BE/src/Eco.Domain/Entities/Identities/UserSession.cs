using Eco.Domain.Common;

namespace Eco.Domain.Entities.Identities;

public class UserSession : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid RefreshTokenId { get; set; }

    public string DeviceId { get; set; }

    public string Browser { get; set; }

    public string OperatingSystem { get; set; }

    public string IpAddress { get; set; }

    public string Location { get; set; }

    public DateTime LastActive { get; set; }

    public DateTime ExpiredAt { get; set; }
}