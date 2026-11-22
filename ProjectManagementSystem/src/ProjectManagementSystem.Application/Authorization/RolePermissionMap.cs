namespace ProjectManagementSystem.Application.Authorization;

public static class RolePermissionMap
{
    private static readonly Dictionary<string, string[]> Map = new(StringComparer.Ordinal)
    {
        [AppRoles.Viewer] =
        [
            AppPermissions.Projects.View,
            AppPermissions.Tasks.View
        ],
        [AppRoles.Member] =
        [
            AppPermissions.Projects.View,
            AppPermissions.Tasks.View,
            AppPermissions.Tasks.Update
        ],
        [AppRoles.TeamLeader] =
        [
            AppPermissions.Projects.View,
            AppPermissions.Tasks.View,
            AppPermissions.Tasks.Create,
            AppPermissions.Tasks.Update,
            AppPermissions.Tasks.Delete
        ],
        [AppRoles.ProjectManager] =
        [
            AppPermissions.Projects.View,
            AppPermissions.Projects.Create,
            AppPermissions.Projects.Update,
            AppPermissions.Tasks.View,
            AppPermissions.Tasks.Create,
            AppPermissions.Tasks.Update,
            AppPermissions.Tasks.Delete
        ],
        [AppRoles.Admin] = AppPermissions.All,
        [AppRoles.SuperAdmin] = AppPermissions.All
    };

    static RolePermissionMap()
    {
        var missing = AppPermissions.All
            .Where(permission => RolesWith(permission).Length == 0)
            .ToArray();

        if (missing.Length > 0)
        {
            throw new InvalidOperationException(
                "Permissions are not assigned to any role: " + string.Join(", ", missing));
        }
    }

    public static IReadOnlyList<string> ForRole(string role)
        => Map.TryGetValue(role, out var permissions) ? permissions : [];

    public static IReadOnlyList<string> ForRoles(IEnumerable<string> roles)
    {
        return roles
            .SelectMany(ForRole)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }

    public static string[] RolesWith(string permission)
    {
        return Map
            .Where(pair => pair.Value.Contains(permission, StringComparer.Ordinal))
            .Select(pair => pair.Key)
            .ToArray();
    }
}
