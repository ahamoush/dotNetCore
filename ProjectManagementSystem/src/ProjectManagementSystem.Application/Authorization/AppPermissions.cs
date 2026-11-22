namespace ProjectManagementSystem.Application.Authorization;

public static class AppPermissions
{
    public static class Projects
    {
        public const string View = "Permissions.Projects.View";
        public const string Create = "Permissions.Projects.Create";
        public const string Update = "Permissions.Projects.Update";
        public const string Delete = "Permissions.Projects.Delete";
    }

    public static class Tasks
    {
        public const string View = "Permissions.Tasks.View";
        public const string Create = "Permissions.Tasks.Create";
        public const string Update = "Permissions.Tasks.Update";
        public const string Delete = "Permissions.Tasks.Delete";
    }

    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string ManageRoles = "Permissions.Users.ManageRoles";
    }

    public static readonly string[] All =
    [
        Projects.View,
        Projects.Create,
        Projects.Update,
        Projects.Delete,
        Tasks.View,
        Tasks.Create,
        Tasks.Update,
        Tasks.Delete,
        Users.View,
        Users.ManageRoles
    ];
}
