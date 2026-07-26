using Eco.Domain.Common;

namespace Eco.Domain.Entities.Identities;

public class User : BaseEntity
{
    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public string PhoneNumber { get; set; }

    public bool EmailVerified { get; set; }

    public bool PhoneVerified { get; set; }

    public bool IsLocked { get; set; }

    public int FailedLoginCount { get; set; }

    public DateTime LastLoginAt { get; set; }
}