using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Users;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Infrastructure.Persistence;

namespace ProjectManagementSystem.Infrastructure.Services;

public class UserAdminService : IUserAdminService
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly ApplicationDbContext _db;

    public UserAdminService(UserManager<ApplicationUser> users, ApplicationDbContext db)
    {
        _users = users;
        _db = db;
    }

    public async Task<IReadOnlyList<UserResponseDto>> GetUsersAsync()
    {
        List<ApplicationUser> users = await _db.Users
            .AsNoTracking()
            .OrderBy(user => user.Email)
            .ToListAsync();

        var result = new List<UserResponseDto>();
        foreach (ApplicationUser user in users)
        {
            IList<string> roles = await _users.GetRolesAsync(user);
            var roleNames = new List<string>();
            foreach (string role in roles)
            {
                if (!roleNames.Contains(role))
                    roleNames.Add(role);
            }

            roleNames.Sort(StringComparer.Ordinal);

            result.Add(new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Roles = roleNames
            });
        }

        return result;
    }

    public IReadOnlyList<RoleResponseDto> GetRoles()
    {
        return AppRoles.All
            .Select(role => new RoleResponseDto
            {
                Name = role,
                Permissions = RolePermissionMap.ForRole(role).ToList()
            })
            .ToList();
    }

    public async Task<UserAdminResult> SetRolesAsync(
        string userId,
        IReadOnlyList<string> roles,
        string actorUserId,
        bool actorIsSuperAdmin)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return UserAdminResult.Invalid("User id is required.");

        if (string.Equals(userId, actorUserId, StringComparison.Ordinal))
            return UserAdminResult.Forbidden("You cannot change your own roles.");

        var requested = new List<string>();
        foreach (string role in roles ?? [])
        {
            string trimmed = role.Trim();
            if (trimmed.Length == 0)
                continue;
            if (!requested.Contains(trimmed))
                requested.Add(trimmed);
        }

        if (requested.Count == 0)
            return UserAdminResult.Invalid("At least one role is required.");

        var unknown = new List<string>();
        foreach (string role in requested)
        {
            if (!AppRoles.All.Contains(role))
                unknown.Add(role);
        }

        if (unknown.Count > 0)
            return UserAdminResult.Invalid("Unknown roles: " + string.Join(", ", unknown) + ".");

        var user = await _users.FindByIdAsync(userId);
        if (user is null)
            return UserAdminResult.NotFound("User not found.");

        IList<string> current = await _users.GetRolesAsync(user);
        bool touchesSuperAdmin = requested.Contains(AppRoles.SuperAdmin)
            || current.Contains(AppRoles.SuperAdmin);

        if (touchesSuperAdmin && !actorIsSuperAdmin)
            return UserAdminResult.Forbidden("Only a SuperAdmin can assign or change the SuperAdmin role.");

        if (current.Contains(AppRoles.SuperAdmin)
            && !requested.Contains(AppRoles.SuperAdmin))
        {
            var superAdmins = await _users.GetUsersInRoleAsync(AppRoles.SuperAdmin);
            if (superAdmins.Count <= 1)
                return UserAdminResult.Forbidden("Cannot remove the last SuperAdmin.");
        }

        var toRemove = new List<string>();
        foreach (string role in current)
        {
            if (!requested.Contains(role))
                toRemove.Add(role);
        }

        var toAdd = new List<string>();
        foreach (string role in requested)
        {
            if (!current.Contains(role))
                toAdd.Add(role);
        }

        if (toRemove.Count == 0 && toAdd.Count == 0)
            return UserAdminResult.Ok();

        await using var transaction = await _db.Database.BeginTransactionAsync();

        if (toRemove.Count > 0)
        {
            IdentityResult remove = await _users.RemoveFromRolesAsync(user, toRemove);
            if (!remove.Succeeded)
                return UserAdminResult.Invalid(JoinErrors(remove));
        }

        if (toAdd.Count > 0)
        {
            IdentityResult add = await _users.AddToRolesAsync(user, toAdd);
            if (!add.Succeeded)
                return UserAdminResult.Invalid(JoinErrors(add));
        }

        await transaction.CommitAsync();
        return UserAdminResult.Ok();
    }

    private static string JoinErrors(IdentityResult result)
    {
        var messages = new List<string>();
        foreach (IdentityError error in result.Errors)
            messages.Add(error.Description);

        return string.Join(" ", messages);
    }
}
