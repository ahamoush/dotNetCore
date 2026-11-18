using ProjectManagementSystem.DTOs.Auth;

namespace ProjectManagementSystem.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto dto);
    Task<AuthResult> LoginAsync(LoginDto dto);
}