namespace ProjectManagementSystem.Application.DTOs.Users;

public class RoleResponseDto
{
    public string Name { get; set; } = string.Empty;

    public IList<string> Permissions { get; set; } = [];
}
