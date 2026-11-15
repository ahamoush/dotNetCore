using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Interfaces;

public interface IJwtTokenService
{
    // ينشئ JWT بعد Login ناجح
    (string Token, DateTime Expiration) CreateToken(
        ApplicationUser user,
        IList<string> roles,
        IList<System.Security.Claims.Claim> extraClaims);
}
