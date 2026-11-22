using FluentValidation;
using ProjectManagementSystem.Application.DTOs.Tasks;

namespace ProjectManagementSystem.Application.Validators.Tasks;

public sealed class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.ProjectId)
            .GreaterThan(0);
    }
}
