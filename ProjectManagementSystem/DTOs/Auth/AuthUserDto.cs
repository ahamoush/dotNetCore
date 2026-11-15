namespace ProjectManagementSystem.DTOs.Auth;

// معلومات المستخدم اللي بنرجعها مع التوكن (من غير Password)
public class AuthUserDto
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public IList<string> Roles { get; set; } = new List<string>();
}
