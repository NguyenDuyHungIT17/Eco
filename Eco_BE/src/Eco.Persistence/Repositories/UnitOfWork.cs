using Eco.Application.Common.Interfaces.Persistence;
using Eco.Domain.Entities.Identities;
using Eco.Persistence.Context;

namespace Eco.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EcoDbContext _context;
    
    private IGenericRepository<User>? _users;
    private IGenericRepository<UserProfile>? _userProfiles;
    private IGenericRepository<Role>? _roles;
    private IGenericRepository<Permission>? _permissions;
    private IGenericRepository<UserRole>? _userRoles;
    private IGenericRepository<RolePermission>? _rolePermissions;
    private IGenericRepository<RefreshToken>? _refreshTokens;
    private IGenericRepository<UserSession>? _userSessions;
    private IGenericRepository<EmailVerification>? _emailVerifications;
    private IGenericRepository<LoginHistory>? _loginHistories;

    public UnitOfWork(EcoDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);
    public IGenericRepository<UserProfile> UserProfiles => _userProfiles ??= new GenericRepository<UserProfile>(_context);
    public IGenericRepository<Role> Roles => _roles ??= new GenericRepository<Role>(_context);
    public IGenericRepository<Permission> Permissions => _permissions ??= new GenericRepository<Permission>(_context);
    public IGenericRepository<UserRole> UserRoles => _userRoles ??= new GenericRepository<UserRole>(_context);
    public IGenericRepository<RolePermission> RolePermissions => _rolePermissions ??= new GenericRepository<RolePermission>(_context);
    public IGenericRepository<RefreshToken> RefreshTokens => _refreshTokens ??= new GenericRepository<RefreshToken>(_context);
    public IGenericRepository<UserSession> UserSessions => _userSessions ??= new GenericRepository<UserSession>(_context);
    public IGenericRepository<EmailVerification> EmailVerifications => _emailVerifications ??= new GenericRepository<EmailVerification>(_context);
    public IGenericRepository<LoginHistory> LoginHistories => _loginHistories ??= new GenericRepository<LoginHistory>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
