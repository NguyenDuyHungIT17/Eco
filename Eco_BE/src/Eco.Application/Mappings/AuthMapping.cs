using Eco.Application.DTOs.Auth;
using Eco.Domain.Entities.Identities;
using Eco.Domain.Enum;

namespace Eco.Application.Mappings;

public static class AuthMapping
{
    public static User ToEntity(this RegisterRequestDto request, string passwordHash, Guid userId, DateTime now)
    {
        return new User
        {
            Id = userId,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PhoneNumber = request.PhoneNumber,
            EmailVerified = false,
            PhoneVerified = false,
            IsLocked = false,
            FailedLoginCount = 0,
            CreatedAt = now,
            IsDeleted = false
        };
    }

    public static UserProfile ToProfileEntity(this RegisterRequestDto request, Guid userId, DateTime now)
    {
        return new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DisplayName = request.FullName,
            FirstName = string.Empty,
            LastName = string.Empty,
            Avatar = string.Empty,
            Gender = Gender.Unknown,
            Birthday = DateOnly.FromDateTime(now.AddYears(-20)),
            Country = string.Empty,
            Timezone = "UTC",
            Language = "vi",
            Bio = string.Empty,
            CreatedAt = now,
            IsDeleted = false
        };
    }

    public static EmailVerification ToEmailVerificationEntity(this RegisterRequestDto request, Guid userId, DateTime now)
    {
        return new EmailVerification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Guid.NewGuid().ToString("N"),
            Status = Status.VerificationStatus.Pending,
            ExpiredAt = now.AddHours(24),
            CreatedAt = now,
            IsDeleted = false
        };
    }

    public static LoginHistory ToLoginHistoryEntity(this LoginRequestDto request, Guid userId, bool success, DateTime now)
    {
        return new LoginHistory
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Browser = request.DeviceInfo,
            OperatingSystem = string.Empty,
            IpAddress = request.IpAddress,
            Location = string.Empty,
            Success = success,
            LoginAt = now,
            CreatedAt = now,
            IsDeleted = false
        };
    }

    public static RefreshToken ToEntity(this string token, Guid userId, DateTime now)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiredAt = now.AddDays(7),
            IsRevoked = false,
            CreatedAt = now,
            IsDeleted = false
        };
    }

    public static UserSession ToEntity(this LoginRequestDto request, Guid userId, Guid refreshTokenId, DateTime now)
    {
        return new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RefreshTokenId = refreshTokenId,
            DeviceId = Guid.NewGuid().ToString(),
            Browser = request.DeviceInfo,
            OperatingSystem = string.Empty,
            IpAddress = request.IpAddress,
            Location = string.Empty,
            LastActive = now,
            ExpiredAt = now.AddDays(7),
            CreatedAt = now,
            IsDeleted = false
        };
    }

    public static AuthResponseDto ToResponse(this (string AccessToken, string RefreshToken) tokens)
    {
        return new AuthResponseDto
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            ExpiresInSeconds = 3600
        };
    }
}