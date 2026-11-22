namespace ProjectManagementSystem.Application.DTOs.Auth;

public class CurrentUserDto
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public IList<string> Roles { get; set; } = [];

    public IList<string> Permissions { get; set; } = [];
}
