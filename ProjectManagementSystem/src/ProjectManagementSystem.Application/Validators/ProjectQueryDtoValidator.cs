using FluentValidation;
using ProjectManagementSystem.Application.DTOs;

namespace ProjectManagementSystem.Application.Validators;

public sealed class ProjectQueryDtoValidator : AbstractValidator<ProjectQueryDto>
{
    private static readonly string[] AllowedSortFields = ["name", "createdat", "duedate"];

    public ProjectQueryDtoValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(200);

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortBy)
            .Must(sortBy => AllowedSortFields.Contains(sortBy.Trim(), StringComparer.OrdinalIgnoreCase))
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage("SortBy must be one of: Name, CreatedAt, DueDate.");
    }
}
