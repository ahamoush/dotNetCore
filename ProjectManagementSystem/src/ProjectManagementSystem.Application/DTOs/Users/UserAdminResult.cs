namespace ProjectManagementSystem.Application.DTOs.Users;

public enum UserAdminStatus
{
    Ok,
    NotFound,
    Invalid,
    Forbidden
}

public class UserAdminResult
{
    public UserAdminStatus Status { get; init; }

    public string? Message { get; init; }

    public static UserAdminResult Ok()
        => new() { Status = UserAdminStatus.Ok };

    public static UserAdminResult NotFound(string message)
        => new() { Status = UserAdminStatus.NotFound, Message = message };

    public static UserAdminResult Invalid(string message)
        => new() { Status = UserAdminStatus.Invalid, Message = message };

    public static UserAdminResult Forbidden(string message)
        => new() { Status = UserAdminStatus.Forbidden, Message = message };
}
