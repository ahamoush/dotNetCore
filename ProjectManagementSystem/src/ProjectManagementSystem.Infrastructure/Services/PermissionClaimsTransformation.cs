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

        var userId = principal.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
            return principal;

        var roles = await _db.UserRoles
            .AsNoTracking()
            .Where(userRole => userRole.UserId == userId)
            .Join(
                _db.Roles.AsNoTracking(),
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => role.Name!)
            .ToListAsync();

        var clone = principal.Clone();
        var identity = (ClaimsIdentity)clone.Identity!;
        var roleClaimType = string.IsNullOrEmpty(identity.RoleClaimType)
            ? AppClaimTypes.Role
            : identity.RoleClaimType;

        foreach (var claim in identity.FindAll(roleClaimType).ToList())
            identity.TryRemoveClaim(claim);

        foreach (var role in roles.Distinct(StringComparer.Ordinal))
            identity.AddClaim(new Claim(roleClaimType, role));

        identity.AddClaim(new Claim(AppClaimTypes.PermissionsSynced, "true"));
        return clone;
    }
}
