namespace ProjectManagementSystem.DTOs.Auth;

// استجابة Login الناجح
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public AuthUserDto User { get; set; } = new();
}
