namespace Eco.Application.DTOs.Auth;

public class LoginRequestDto
{
    public string UsernameOrEmail { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string DeviceInfo { get; set; } = default!;
    public string IpAddress { get; set; } = default!;
}
