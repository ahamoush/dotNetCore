using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Configuration;
using ProjectManagementSystem.DTOs.Auth;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

// منطق Register و Login
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(bool Succeeded, RegisterResponseDto? Data, IEnumerable<string> Errors)>
        RegisterAsync(RegisterDto dto)
    {
        // 1) هل الـ Username موجود؟
        var existingByName = await _userManager.FindByNameAsync(dto.UserName);
        if (existingByName is not null)
        {
            return (false, null, ["Username is already taken."]);
        }

        // 2) هل الـ Email موجود؟
        var existingByEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (existingByEmail is not null)
        {
            return (false, null, ["Email is already registered."]);
        }

        // 3) إنشاء المستخدم
        // Identity هيعمل Password Hashing تلقائياً (مش plain text)
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => e.Description);
            return (false, null, errors);
        }

        // 4) التسجيل العام يعطي دور User فقط (مش Admin/SuperAdmin)
        if (await _roleManager.RoleExistsAsync(AppRoles.User))
        {
            await _userManager.AddToRoleAsync(user, AppRoles.User);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var response = new RegisterResponseDto
        {
            Message = "User registered successfully.",
            User = new AuthUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = roles
            }
        };

        return (true, response, []);
    }

    public async Task<(bool Succeeded, LoginResponseDto? Data, string? Error)>
        LoginAsync(LoginDto dto)
    {
        // 1) نبحث بالـ Username أو Email
        var user = await FindByUserNameOrEmailAsync(dto.UserNameOrEmail);

        // رسالة عامة عشان ما نكشفش هل اليوزر موجود ولا لأ
        if (user is null)
        {
            return (false, null, "Invalid username/email or password.");
        }

        // 2) الحساب لازم يكون Active
        if (!user.IsActive)
        {
            return (false, null, "This account is inactive.");
        }

        // 3) التحقق من كلمة المرور (Identity يقارن الـ Hash)
        var passwordOk = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordOk)
        {
            return (false, null, "Invalid username/email or password.");
        }

        // 4) نجيب الأدوار و Claims
        var roles = await _userManager.GetRolesAsync(user);
        var permissionClaims = await GetPermissionClaimsAsync(user, roles);

        // 5) إنشاء JWT
        var (token, expiration) = _jwtTokenService.CreateToken(
            user,
            roles,
            permissionClaims);

        var response = new LoginResponseDto
        {
            Token = token,
            Expiration = expiration,
            User = new AuthUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Roles = roles
            }
        };

        return (true, response, null);
    }

    private async Task<ApplicationUser?> FindByUserNameOrEmailAsync(string userNameOrEmail)
    {
        var value = userNameOrEmail.Trim();

        // لو فيه @ غالباً Email
        if (value.Contains('@'))
        {
            var byEmail = await _userManager.FindByEmailAsync(value);
            if (byEmail is not null)
                return byEmail;
        }

        var byName = await _userManager.FindByNameAsync(value);
        if (byName is not null)
            return byName;

        // محاولة أخيرة: بحث بالإيميل حتى لو مفيش @
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == value);
    }

    // نجيب permission claims من RoleClaims (+ أي UserClaims مباشرة)
    private async Task<List<Claim>> GetPermissionClaimsAsync(
        ApplicationUser user,
        IList<string> roles)
    {
        var claims = new List<Claim>();

        // Claims مباشرة على المستخدم
        var userClaims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims)
        {
            if (claim.Type == AppClaimTypes.Permission)
                claims.Add(claim);
        }

        // Claims من كل Role
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
                continue;

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims)
            {
                if (claim.Type == AppClaimTypes.Permission)
                    claims.Add(claim);
            }
        }

        // نشيل التكرار
        return claims
            .GroupBy(c => $"{c.Type}:{c.Value}")
            .Select(g => g.First())
            .ToList();
    }
}
