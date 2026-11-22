using ProjectManagementSystem.Application.DTOs;

namespace ProjectManagementSystem.Application.Validators;

public static class ProjectQueryValidator
{
    public static bool HasValidPaging(ProjectQueryDto query)
        => query.PageNumber >= 1 && query.PageSize >= 1 && query.PageSize <= 100;
}
