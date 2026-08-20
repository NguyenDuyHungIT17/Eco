using Eco.Application.Common.Interfaces.Identity;
using Eco.Application.Common.Interfaces.Persistence;
using Eco.Application.DTOs.Auth;
using Eco.Domain.Entities.Identities;
using Eco.Domain.Enum;

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

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        bool exists = await _unitOfWork.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email);
        if (exists)
        {
            throw new ArgumentException("Username or Email already exists.");
        }

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            PhoneNumber = request.PhoneNumber,
            EmailVerified = false,
            PhoneVerified = false,
            IsLocked = false,
            FailedLoginCount = 0,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var profile = new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DisplayName = request.FullName,
            FirstName = string.Empty,
            LastName = string.Empty,
            Avatar = string.Empty,
            Gender = Gender.Unknown,
            Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
            Country = string.Empty,
            Timezone = "UTC",
            Language = "vi",
            Bio = string.Empty,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var emailVerification = new EmailVerification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Guid.NewGuid().ToString("N"),
            Status = Status.VerificationStatus.Pending,
            ExpiredAt = DateTime.UtcNow.AddHours(24),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _unitOfWork.Users.Add(user);
        _unitOfWork.UserProfiles.Add(profile);
        _unitOfWork.EmailVerifications.Add(emailVerification);

        await _unitOfWork.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user, Array.Empty<string>(), Array.Empty<string>());
        var refreshTokenStr = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenStr,
            ExpiredAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _unitOfWork.RefreshTokens.Add(refreshToken);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenStr,
            ExpiresInSeconds = 3600
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
            
            var failureHistory = new LoginHistory
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Browser = request.DeviceInfo,
                OperatingSystem = string.Empty,
                IpAddress = request.IpAddress,
                Location = string.Empty,
                Success = false,
                LoginAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _unitOfWork.LoginHistories.Add(failureHistory);
            await _unitOfWork.SaveChangesAsync();

            throw new ArgumentException("Invalid username or password.");
        }

        user.FailedLoginCount = 0;
        user.LastLoginAt = DateTime.UtcNow;

        var successHistory = new LoginHistory
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Browser = request.DeviceInfo,
            OperatingSystem = string.Empty,
            IpAddress = request.IpAddress,
            Location = string.Empty,
            Success = true,
            LoginAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
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

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenStr,
            ExpiredAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var userSession = new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RefreshTokenId = refreshToken.Id,
            DeviceId = Guid.NewGuid().ToString(),
            Browser = request.DeviceInfo,
            OperatingSystem = string.Empty,
            IpAddress = request.IpAddress,
            Location = string.Empty,
            LastActive = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _unitOfWork.RefreshTokens.Add(refreshToken);
        _unitOfWork.UserSessions.Add(userSession);

        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenStr,
            ExpiresInSeconds = 3600
        };
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

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshTokenStr,
            ExpiredAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _unitOfWork.RefreshTokens.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenStr,
            ExpiresInSeconds = 3600
        };
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
