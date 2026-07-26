using Eco.Domain.Common;

namespace Eco.Domain.Entities.Identities;

// bảng trung gian giữa User và Role
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public DateTime AssignedAt { get; set; }
}