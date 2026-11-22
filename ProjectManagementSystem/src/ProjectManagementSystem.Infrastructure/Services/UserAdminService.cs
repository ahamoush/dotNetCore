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
        var users = await _db.Users
            .AsNoTracking()
            .OrderBy(user => user.Email)
            .Select(user => new
            {
                user.Id,
                Email = user.Email ?? string.Empty,
                user.FullName
            })
            .ToListAsync();

        var roleRows = await (
            from userRole in _db.UserRoles.AsNoTracking()
            join role in _db.Roles.AsNoTracking() on userRole.RoleId equals role.Id
            select new { userRole.UserId, role.Name }
        ).ToListAsync();

        var rolesByUser = roleRows
            .Where(row => !string.IsNullOrWhiteSpace(row.Name))
            .GroupBy(row => row.UserId)
            .ToDictionary(
                group => group.Key,
                group => (IList<string>)group
                    .Select(row => row.Name!)
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(name => name, StringComparer.Ordinal)
                    .ToList());

        return users
            .Select(user => new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = rolesByUser.TryGetValue(user.Id, out var roles) ? roles : []
            })
            .ToList();
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

        var requested = (roles ?? [])
            .Select(role => role.Trim())
            .Where(role => role.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (requested.Length == 0)
            return UserAdminResult.Invalid("At least one role is required.");

        var unknown = requested
            .Where(role => !AppRoles.All.Contains(role, StringComparer.Ordinal))
            .ToArray();

        if (unknown.Length > 0)
            return UserAdminResult.Invalid("Unknown roles: " + string.Join(", ", unknown) + ".");

        var user = await _users.FindByIdAsync(userId);
        if (user is null)
            return UserAdminResult.NotFound("User not found.");

        var current = (await _users.GetRolesAsync(user)).ToArray();
        var touchesSuperAdmin = requested.Contains(AppRoles.SuperAdmin, StringComparer.Ordinal)
            || current.Contains(AppRoles.SuperAdmin, StringComparer.Ordinal);

        if (touchesSuperAdmin && !actorIsSuperAdmin)
            return UserAdminResult.Forbidden("Only a SuperAdmin can assign or change the SuperAdmin role.");

        if (current.Contains(AppRoles.SuperAdmin, StringComparer.Ordinal)
            && !requested.Contains(AppRoles.SuperAdmin, StringComparer.Ordinal))
        {
            var superAdmins = await _users.GetUsersInRoleAsync(AppRoles.SuperAdmin);
            if (superAdmins.Count <= 1)
                return UserAdminResult.Forbidden("Cannot remove the last SuperAdmin.");
        }

        var toRemove = current
            .Where(role => !requested.Contains(role, StringComparer.Ordinal))
            .ToArray();
        var toAdd = requested
            .Where(role => !current.Contains(role, StringComparer.Ordinal))
            .ToArray();

        if (toRemove.Length == 0 && toAdd.Length == 0)
            return UserAdminResult.Ok();

        await using var transaction = await _db.Database.BeginTransactionAsync();

        if (toRemove.Length > 0)
        {
            var remove = await _users.RemoveFromRolesAsync(user, toRemove);
            if (!remove.Succeeded)
                return UserAdminResult.Invalid(string.Join(" ", remove.Errors.Select(error => error.Description)));
        }

        if (toAdd.Length > 0)
        {
            var add = await _users.AddToRolesAsync(user, toAdd);
            if (!add.Succeeded)
                return UserAdminResult.Invalid(string.Join(" ", add.Errors.Select(error => error.Description)));
        }

        await transaction.CommitAsync();
        return UserAdminResult.Ok();
    }
}
