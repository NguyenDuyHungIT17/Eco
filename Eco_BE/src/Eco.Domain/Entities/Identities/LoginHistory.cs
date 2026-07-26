using Eco.Domain.Common;

namespace Eco.Domain.Entities.Identities;

public class LoginHistory : BaseEntity
{
    public Guid UserId { get; set; }

    public string Browser { get; set; }

    public string OperatingSystem { get; set; }

    public string IpAddress { get; set; }

    public string Location { get; set; }

    public bool Success { get; set; }

    public DateTime LoginAt { get; set; }
}