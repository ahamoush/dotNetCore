using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProjectManagementSystem.Configuration;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

// إنشاء الأدوار + Claims + SuperAdmin مرة واحدة (Idempotent)
// يعني إعادة تشغيل التطبيق مش هتعمل مستخدمين مكررين
public class IdentitySeedService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SeedSettings _seedSettings;
    private readonly ILogger<IdentitySeedService> _logger;

    public IdentitySeedService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<SeedSettings> seedSettings,
        ILogger<IdentitySeedService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _seedSettings = seedSettings.Value;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedRoleClaimsAsync();
        await SeedSuperAdminAsync();
    }

    private async Task SeedRolesAsync()
    {
        foreach (var roleName in AppRoles.All)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                continue;

            var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
            if (result.Succeeded)
            {
                _logger.LogInformation("Created role: {RoleName}", roleName);
            }
            else
            {
                _logger.LogError(
                    "Failed to create role {RoleName}: {Errors}",
                    roleName,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

    private async Task SeedRoleClaimsAsync()
    {
        // SuperAdmin: كل الصلاحيات
        await EnsureRoleClaimsAsync(
            AppRoles.SuperAdmin,
            AppPermissions.UsersAll.Concat(AppPermissions.ProductsAll).ToArray());

        // Admin: إدارة users + products (بدون delete users كمثال بسيط للفرق)
        await EnsureRoleClaimsAsync(
            AppRoles.Admin,
            [
                AppPermissions.UsersRead,
                AppPermissions.UsersCreate,
                AppPermissions.UsersUpdate,
                AppPermissions.ProductsRead,
                AppPermissions.ProductsCreate,
                AppPermissions.ProductsUpdate,
                AppPermissions.ProductsDelete
            ]);

        // User: قراءة فقط
        await EnsureRoleClaimsAsync(
            AppRoles.User,
            [
                AppPermissions.ProductsRead
            ]);
    }

    private async Task EnsureRoleClaimsAsync(string roleName, string[] permissions)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role is null)
            return;

        var existingClaims = await _roleManager.GetClaimsAsync(role);

        foreach (var permission in permissions)
        {
            var alreadyExists = existingClaims.Any(c =>
                c.Type == AppClaimTypes.Permission &&
                c.Value == permission);

            if (alreadyExists)
                continue;

            await _roleManager.AddClaimAsync(
                role,
                new Claim(AppClaimTypes.Permission, permission));
        }
    }

    private async Task SeedSuperAdminAsync()
    {
        // لازم Email و Password يكونوا موجودين في الإعدادات/User Secrets
        if (string.IsNullOrWhiteSpace(_seedSettings.SuperAdminEmail) ||
            string.IsNullOrWhiteSpace(_seedSettings.SuperAdminPassword))
        {
            _logger.LogWarning(
                "SuperAdmin seeding skipped. Set Seed:SuperAdminEmail and Seed:SuperAdminPassword in User Secrets.");
            return;
        }

        var userName = string.IsNullOrWhiteSpace(_seedSettings.SuperAdminUserName)
            ? "abd"
            : _seedSettings.SuperAdminUserName.Trim();

        // لو المستخدم موجود بالفعل: نحافظ عليه (مش هنغير الباسورد)
        var existingUser = await _userManager.FindByNameAsync(userName);
        if (existingUser is not null)
        {
            if (!await _userManager.IsInRoleAsync(existingUser, AppRoles.SuperAdmin))
            {
                await _userManager.AddToRoleAsync(existingUser, AppRoles.SuperAdmin);
            }

            _logger.LogInformation(
                "SuperAdmin user '{UserName}' already exists. Password was not changed.",
                userName);
            return;
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = _seedSettings.SuperAdminEmail.Trim(),
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // CreateAsync يعمل hashing لكلمة المرور تلقائياً
        var createResult = await _userManager.CreateAsync(
            user,
            _seedSettings.SuperAdminPassword);

        if (!createResult.Succeeded)
        {
            _logger.LogError(
                "Failed to create SuperAdmin: {Errors}",
                string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return;
        }

        await _userManager.AddToRoleAsync(user, AppRoles.SuperAdmin);

        _logger.LogInformation("SuperAdmin user '{UserName}' created successfully.", userName);
    }
}
