using FluentValidation;
using ProjectManagementSystem.Application.DTOs.Projects;

namespace ProjectManagementSystem.Application.Validators.Projects;

public sealed class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
