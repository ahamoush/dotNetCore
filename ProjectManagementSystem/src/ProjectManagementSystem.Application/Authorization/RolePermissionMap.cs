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

    public static void EnsureEveryPermissionHasARole()
    {
        var missing = new List<string>();
        foreach (string permission in AppPermissions.All)
        {
            if (RolesWith(permission).Length == 0)
                missing.Add(permission);
        }

        if (missing.Count > 0)
        {
            throw new InvalidOperationException(
                "Permissions are not assigned to any role: " + string.Join(", ", missing));
        }
    }

    public static IReadOnlyList<string> ForRole(string role)
    {
        if (Map.TryGetValue(role, out string[]? permissions))
            return permissions;

        return [];
    }

    public static IReadOnlyList<string> ForRoles(IEnumerable<string> roles)
    {
        var permissions = new List<string>();
        foreach (string role in roles)
        {
            foreach (string permission in ForRole(role))
            {
                if (!permissions.Contains(permission))
                    permissions.Add(permission);
            }
        }

        return permissions;
    }

    public static string[] RolesWith(string permission)
    {
        var roles = new List<string>();
        foreach (KeyValuePair<string, string[]> pair in Map)
        {
            foreach (string allowed in pair.Value)
            {
                if (allowed == permission)
                {
                    roles.Add(pair.Key);
                    break;
                }
            }
        }

        return roles.ToArray();
    }
}
