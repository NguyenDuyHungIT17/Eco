using Eco.Domain.Common;
using Eco.Domain.Enum;

namespace Eco.Domain.Entities.Identities;

public class AuditLog : BaseEntity
{
    public Guid UserId { get; set; }

    public Enum.AuditLog.AuditAction Action { get; set; }
    public string Description { get; set; }

    public string IpAddress { get; set; }
}