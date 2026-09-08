using Eco.Application.Common.Interfaces.Identity;
using Eco.Application.Common.Interfaces.Persistence;
using Eco.Application.DTOs.Auth;
using Eco.Application.Mappings;

namespace Eco.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthenticationService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        bool exists = await _unitOfWork.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email);
        if (exists)
        {
            throw new ArgumentException("Username or Email already exists.");
        }

        var userId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var user = request.ToEntity(_passwordHasher.Hash(request.Password), userId, now);
        var profile = request.ToProfileEntity(userId, now);
        var emailVerification = request.ToEmailVerificationEntity(userId, now);

        _unitOfWork.Users.Add(user);
        _unitOfWork.UserProfiles.Add(profile);
        _unitOfWork.EmailVerifications.Add(emailVerification);

        await _unitOfWork.SaveChangesAsync();

        //var accessToken = _tokenService.GenerateAccessToken(user, Array.Empty<string>(), Array.Empty<string>());
        //var refreshTokenStr = _tokenService.GenerateRefreshToken();

        //var refreshToken = refreshTokenStr.ToEntity(userId, DateTime.UtcNow);

        //_unitOfWork.RefreshTokens.Add(refreshToken);
        //await _unitOfWork.SaveChangesAsync();

        return new RegisterResponseDto
        {
            Message = "Registration successful. Please verify your email."
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);
        
        if (user == null)
        {
            throw new ArgumentException("Invalid username or password.");
        }

        if (user.IsLocked)
        {
            throw new InvalidOperationException("Account is locked.");
        }

        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        
        if (!isPasswordValid)
        {
            user.FailedLoginCount++;
            if (user.FailedLoginCount >= 5)
            {
                user.IsLocked = true;
            }
            
            var failureHistory = request.ToLoginHistoryEntity(user.Id, false, DateTime.UtcNow);
            _unitOfWork.LoginHistories.Add(failureHistory);
            await _unitOfWork.SaveChangesAsync();

            throw new ArgumentException("Invalid username or password.");
        }

        user.FailedLoginCount = 0;
        user.LastLoginAt = DateTime.UtcNow;

        var successHistory = request.ToLoginHistoryEntity(user.Id, true, DateTime.UtcNow);
        _unitOfWork.LoginHistories.Add(successHistory);

        var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserId == user.Id);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
        
        var rolesList = await _unitOfWork.Roles.FindAsync(r => roleIds.Contains(r.Id));
        var roles = rolesList.Select(r => r.Code).ToList();

        var rolePermissions = await _unitOfWork.RolePermissions.FindAsync(rp => roleIds.Contains(rp.RoleId));
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

        var permissionsList = await _unitOfWork.Permissions.FindAsync(p => permissionIds.Contains(p.Id));
        var permissions = permissionsList.Select(p => p.Code).Distinct().ToList();

        var accessToken = _tokenService.GenerateAccessToken(user, roles, permissions);
        var refreshTokenStr = _tokenService.GenerateRefreshToken();

        var refreshToken = refreshTokenStr.ToEntity(user.Id, DateTime.UtcNow);
        var userSession = request.ToEntity(user.Id, refreshToken.Id, DateTime.UtcNow);

        _unitOfWork.RefreshTokens.Add(refreshToken);
        _unitOfWork.UserSessions.Add(userSession);

        await _unitOfWork.SaveChangesAsync();

        return (accessToken, refreshTokenStr).ToResponse();
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var refreshToken = await _unitOfWork.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == request.RefreshToken);

        if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiredAt <= DateTime.UtcNow)
        {
            throw new ArgumentException("Invalid or expired refresh token.");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId);
        if (user == null || user.IsLocked)
        {
            throw new ArgumentException("User not found or account locked.");
        }

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;

        var userRoles = await _unitOfWork.UserRoles.FindAsync(ur => ur.UserId == user.Id);
        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
        
        var rolesList = await _unitOfWork.Roles.FindAsync(r => roleIds.Contains(r.Id));
        var roles = rolesList.Select(r => r.Code).ToList();

        var rolePermissions = await _unitOfWork.RolePermissions.FindAsync(rp => roleIds.Contains(rp.RoleId));
        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

        var permissionsList = await _unitOfWork.Permissions.FindAsync(p => permissionIds.Contains(p.Id));
        var permissions = permissionsList.Select(p => p.Code).Distinct().ToList();

        var newAccessToken = _tokenService.GenerateAccessToken(user, roles, permissions);
        var newRefreshTokenStr = _tokenService.GenerateRefreshToken();

        var newRefreshToken = newRefreshTokenStr.ToEntity(user.Id, DateTime.UtcNow);

        _unitOfWork.RefreshTokens.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        return (newAccessToken, newRefreshTokenStr).ToResponse();
    }

    public async Task<bool> RevokeTokenAsync(string tokenStr)
    {
        var refreshToken = await _unitOfWork.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == tokenStr);

        if (refreshToken == null || refreshToken.IsRevoked)
        {
            return false;
        }

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
