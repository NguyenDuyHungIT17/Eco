using Eco.Application.Common.Interfaces.Identity;
using Eco.Application.Common.Responses;
using Eco.Application.Common.Results;
using Eco.Application.DTOs.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Eco.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IValidator<RegisterRequestDto> _registerValidator;
    private readonly IValidator<LoginRequestDto> _loginValidator;

    public AuthController(
        IAuthenticationService authService,
        IValidator<RegisterRequestDto> registerValidator,
        IValidator<LoginRequestDto> loginValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail(
                "Validation failed.",
                "VALIDATION_ERROR",
                validationResult.Errors.Select(error => error.ErrorMessage).ToList()));
        }

        var result = await _authService.RegisterAsync(request);
        if (!result.Success)
        {
            return Conflict(ApiResponse<RegisterResponseDto>.Fail(result.ErrorMessage!, result.ErrorCode));
        }

        var responseData = result.Data!;
        return Ok(ApiResponse<RegisterResponseDto>.Ok(responseData, responseData.Message));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail(
                "Validation failed.",
                "VALIDATION_ERROR",
                validationResult.Errors.Select(error => error.ErrorMessage).ToList()));
        }

        var result = await _authService.LoginAsync(request);
        if (!result.Success)
        {
            var statusCode = result.ErrorCode == nameof(AuthErrorType.AccountLocked)
                ? StatusCodes.Status403Forbidden
                : StatusCodes.Status401Unauthorized;

            return StatusCode(statusCode, ApiResponse<AuthResponseDto>.Fail(result.ErrorMessage!, result.ErrorCode));
        }

        return Ok(ApiResponse<AuthResponseDto>.Ok(result.Data!));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if (!result.Success)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.Fail(result.ErrorMessage!, result.ErrorCode));
        }

        return Ok(ApiResponse<AuthResponseDto>.Ok(result.Data!));
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] string token)
    {
        var result = await _authService.RevokeTokenAsync(token);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<bool>.Fail(result.ErrorMessage!, result.ErrorCode));
        }

        return Ok(ApiResponse<bool>.Ok(result.Data!, "Token revoked successfully."));
    }
}
