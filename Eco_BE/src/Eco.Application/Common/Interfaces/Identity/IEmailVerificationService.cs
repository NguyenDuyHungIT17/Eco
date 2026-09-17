using Eco.Application.DTOs.Auth;
using Eco.Application.Common.Results;

namespace Eco.Application.Common.Interfaces.Identity;

public interface IEmailVerificationService
{
    Task<Result<VerifyEmailResponseDto>> VerifyEmailAsync(string token);
}
