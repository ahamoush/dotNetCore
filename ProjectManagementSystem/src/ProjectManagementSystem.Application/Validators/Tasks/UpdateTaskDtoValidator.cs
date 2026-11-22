using FluentValidation;
using ProjectManagementSystem.Application.DTOs.Tasks;

namespace ProjectManagementSystem.Application.Validators.Tasks;

public sealed class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
