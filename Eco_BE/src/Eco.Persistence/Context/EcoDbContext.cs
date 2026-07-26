using Eco.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;

namespace Eco.Persistence.Context;

public class EcoDbContext : DbContext
{
    public EcoDbContext(DbContextOptions<EcoDbContext> options)
        : base(options)
    {
    }

    #region Identity

    public DbSet<User> Users => Set<User>();

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<UserSession> UserSessions => Set<UserSession>();

    public DbSet<ExternalLogin> ExternalLogins => Set<ExternalLogin>();

    public DbSet<EmailVerification> EmailVerifications => Set<EmailVerification>();

    public DbSet<PasswordReset> PasswordResets => Set<PasswordReset>();

    public DbSet<Otp> Otps => Set<Otp>();

    public DbSet<LoginHistory> LoginHistories => Set<LoginHistory>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcoDbContext).Assembly);
    }
}