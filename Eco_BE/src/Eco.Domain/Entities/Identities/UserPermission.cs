using Eco.Domain.Common;

namespace Eco.Domain.Entities.Identities;

// bảng trung gian giữa Role và Permission
public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }
}