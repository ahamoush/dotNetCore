using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Infrastructure.Persistence;

namespace ProjectManagementSystem.Infrastructure.Services;

public class PermissionClaimsTransformation : IClaimsTransformation
{
    private readonly ApplicationDbContext _db;

    public PermissionClaimsTransformation(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
            return principal;

        if (principal.HasClaim(AppClaimTypes.PermissionsSynced, "true"))
            return principal;

        string? userId = principal.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return principal;

        List<string> roleIds = await _db.UserRoles
            .AsNoTracking()
            .Where(link => link.UserId == userId)
            .Select(link => link.RoleId)
            .ToListAsync();

        List<string> roleNames = await _db.Roles
            .AsNoTracking()
            .Where(role => roleIds.Contains(role.Id) && role.Name != null)
            .Select(role => role.Name!)
            .ToListAsync();

        ClaimsPrincipal clone = principal.Clone();
        var identity = (ClaimsIdentity)clone.Identity!;

        string roleClaimType = AppClaimTypes.Role;
        if (!string.IsNullOrEmpty(identity.RoleClaimType))
            roleClaimType = identity.RoleClaimType;

        List<Claim> oldRoleClaims = identity.FindAll(roleClaimType).ToList();
        foreach (Claim oldClaim in oldRoleClaims)
            identity.RemoveClaim(oldClaim);

        foreach (string roleName in roleNames)
            identity.AddClaim(new Claim(roleClaimType, roleName));

        identity.AddClaim(new Claim(AppClaimTypes.PermissionsSynced, "true"));
        return clone;
    }
}
