using ProjectManagementSystem.DTOs.Auth;

namespace ProjectManagementSystem.Interfaces;

public interface IAuthService
{
    Task<(bool Succeeded, RegisterResponseDto? Data, IEnumerable<string> Errors)>
        RegisterAsync(RegisterDto dto);

    Task<(bool Succeeded, LoginResponseDto? Data, string? Error)>
        LoginAsync(LoginDto dto);
}
