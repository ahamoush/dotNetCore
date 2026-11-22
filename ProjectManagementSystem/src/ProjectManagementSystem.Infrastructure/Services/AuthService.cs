using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Auth;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Infrastructure.Persistence;

namespace ProjectManagementSystem.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> users, IConfiguration configuration)
    {
        _users = users;
        _configuration = configuration;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var create = await _users.CreateAsync(user, dto.Password);
        if (!create.Succeeded)
            return AuthResult.Failure(create.Errors.Select(e => e.Description).ToArray());

        var addRole = await _users.AddToRoleAsync(user, AppRoles.Member);
        if (!addRole.Succeeded)
            return AuthResult.Failure(addRole.Errors.Select(e => e.Description).ToArray());

        return AuthResult.Success(await CreateResponseAsync(user));
    }

    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var user = await _users.FindByEmailAsync(dto.Email);
        if (user is null || !await _users.CheckPasswordAsync(user, dto.Password))
            return AuthResult.Failure("Invalid email or password.");

        return AuthResult.Success(await CreateResponseAsync(user));
    }

    private async Task<AuthResponseDto> CreateResponseAsync(ApplicationUser user)
    {
        var roles = await _users.GetRolesAsync(user);
        var permissions = RolePermissionMap.ForRoles(roles);
        var expiresAt = DateTime.UtcNow.AddMinutes(ReadExpiresInMinutes());

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(AppClaimTypes.Name, user.FullName)
        };

        foreach (string role in roles)
            claims.Add(new Claim(AppClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ReadKey()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            Roles = roles,
            Permissions = permissions.ToList()
        };
    }

    private string ReadKey()
    {
        var key = _configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(key) || key.Length < 32)
            throw new InvalidOperationException("Jwt:Key must be at least 32 characters.");

        return key;
    }

    private int ReadExpiresInMinutes()
    {
        string? text = _configuration["Jwt:ExpiresInMinutes"];
        if (int.TryParse(text, out int minutes) && minutes > 0)
            return minutes;

        return 60;
    }
}
