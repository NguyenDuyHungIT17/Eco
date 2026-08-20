using Eco.Domain.Entities.Identities;

namespace Eco.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<User> Users { get; }
    IGenericRepository<UserProfile> UserProfiles { get; }
    IGenericRepository<Role> Roles { get; }
    IGenericRepository<Permission> Permissions { get; }
    IGenericRepository<UserRole> UserRoles { get; }
    IGenericRepository<RolePermission> RolePermissions { get; }
    IGenericRepository<RefreshToken> RefreshTokens { get; }
    IGenericRepository<UserSession> UserSessions { get; }
    IGenericRepository<EmailVerification> EmailVerifications { get; }
    IGenericRepository<LoginHistory> LoginHistories { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
