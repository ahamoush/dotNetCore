namespace ProjectManagementSystem.Configuration;

// أسماء الأدوار في مكان واحد عشان ما نكتبش strings غلط
public static class AppRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string User = "User";

    public static readonly string[] All =
    [
        SuperAdmin,
        Admin,
        User
    ];
}
