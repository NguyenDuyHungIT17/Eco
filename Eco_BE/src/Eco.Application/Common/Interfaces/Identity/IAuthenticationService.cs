using Eco.Application.DTOs.Auth;
using Eco.Application.Common.Results;

namespace Eco.Application.Common.Interfaces.Identity;

public interface IAuthenticationService
{
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request);
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<Result<bool>> RevokeTokenAsync(string refreshToken);
}
