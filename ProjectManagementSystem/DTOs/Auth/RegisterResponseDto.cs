namespace ProjectManagementSystem.DTOs.Auth;

// استجابة Register الناجح
public class RegisterResponseDto
{
    public string Message { get; set; } = string.Empty;

    public AuthUserDto User { get; set; } = new();
}
