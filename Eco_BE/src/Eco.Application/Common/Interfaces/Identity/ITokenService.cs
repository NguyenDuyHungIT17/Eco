using System.Security.Claims;
using Eco.Domain.Entities.Identities;

namespace Eco.Application.Common.Interfaces.Identity;

public interface ITokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
