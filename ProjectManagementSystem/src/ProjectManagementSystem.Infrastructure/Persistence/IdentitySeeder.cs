using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementSystem.Application.Authorization;

namespace ProjectManagementSystem.Infrastructure.Persistence;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        await SeedRolesAsync(services);
        await SeedSuperAdminAsync(services, configuration);
    }

    private static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in AppRoles.All)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    $"Failed to create role '{roleName}': {errors}");
            }
        }
    }

    private static async Task SeedSuperAdminAsync(IServiceProvider services, IConfiguration configuration)
    {
        var email = configuration["Seed:SuperAdmin:Email"];
        var password = configuration["Seed:SuperAdmin:Password"];
        var fullName = configuration["Seed:SuperAdmin:FullName"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await users.FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = string.IsNullOrWhiteSpace(fullName) ? "Super Admin" : fullName,
                EmailConfirmed = true
            };

            var create = await users.CreateAsync(user, password);
            if (!create.Succeeded)
            {
                var errors = string.Join(", ", create.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create SuperAdmin '{email}': {errors}");
            }
        }

        if (!await users.IsInRoleAsync(user, AppRoles.SuperAdmin))
        {
            var addRole = await users.AddToRoleAsync(user, AppRoles.SuperAdmin);
            if (!addRole.Succeeded)
            {
                var errors = string.Join(", ", addRole.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign SuperAdmin to '{email}': {errors}");
            }
        }
    }
}
