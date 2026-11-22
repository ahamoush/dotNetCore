namespace ProjectManagementSystem.Infrastructure.Persistence;

public static class AppRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string ProjectManager = "ProjectManager";
    public const string TeamLeader = "TeamLeader";
    public const string Member = "Member";
    public const string Viewer = "Viewer";

    public static readonly string[] All =
    [
        SuperAdmin,
        Admin,
        ProjectManager,
        TeamLeader,
        Member,
        Viewer
    ];
}
