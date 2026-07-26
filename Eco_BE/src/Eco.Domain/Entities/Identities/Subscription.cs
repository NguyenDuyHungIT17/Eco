using Eco.Domain.Common;
using Eco.Domain.Enum;

namespace Eco.Domain.Entities.Identities;

public class Subscription : BaseEntity
{
    public Guid UserId { get; set; }

    public SubscriptionEnum.SubscriptionPlan Plan { get; set; } = default!;

    public SubscriptionEnum.SubscriptionStatus Status { get; set; }
    public int AiCredits { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public bool IsActive { get; set; }
}