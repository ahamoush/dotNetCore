using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Configuration;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

// خدمة إنشاء JWT بعد Login ناجح
public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public (string Token, DateTime Expiration) CreateToken(
        ApplicationUser user,
        IList<string> roles,
        IList<Claim> extraClaims)
    {
        // Claims أساسية عن المستخدم
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // ClaimTypes.NameIdentifier مهم لـ ASP.NET عشان يعرف User.Id
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        // نضيف كل Role كـ Claim
        // [Authorize(Roles = "Admin")] يعتمد على ClaimTypes.Role
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // نضيف Claims إضافية (مثل permissions من RoleClaims)
        claims.AddRange(extraClaims);

        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

        // المفتاح السري لتوقيع التوكن (HMAC SHA-256)
        var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiration);
    }
}
