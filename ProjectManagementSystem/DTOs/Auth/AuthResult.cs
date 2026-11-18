namespace ProjectManagementSystem.DTOs.Auth;

public class AuthResult
{
    public bool Succeeded { get; init; }
    public AuthResponseDto? Data { get; init; }
    public string[] Errors { get; init; } = [];

    public static AuthResult Success(AuthResponseDto data)
        => new() { Succeeded = true, Data = data };

    public static AuthResult Failure(params string[] errors)
        => new() { Succeeded = false, Errors = errors };
}