using ProjectManagementSystem.Application.DTOs.Auth;

namespace ProjectManagementSystem.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto dto);

    Task<AuthResult> LoginAsync(LoginDto dto);
}
