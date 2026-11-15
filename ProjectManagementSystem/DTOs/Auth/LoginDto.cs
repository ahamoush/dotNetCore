using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.DTOs.Auth;

// بيانات تسجيل الدخول (Username أو Email + Password)
public class LoginDto
{
    [Required]
    public string UserNameOrEmail { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
