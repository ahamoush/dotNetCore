using FluentValidation;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Users;

namespace ProjectManagementSystem.Application.Validators.Users;

public sealed class SetUserRolesDtoValidator : AbstractValidator<SetUserRolesDto>
{
    public SetUserRolesDtoValidator()
    {
        RuleFor(x => x.Roles)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(roles => roles.Distinct(StringComparer.OrdinalIgnoreCase).Count() == roles.Count)
            .WithMessage("Roles must not contain duplicates.");

        RuleForEach(x => x.Roles)
            .NotEmpty()
            .Must(role => AppRoles.All.Contains(role, StringComparer.Ordinal))
            .WithMessage("'{PropertyValue}' is not a valid role.")
            .When(x => x.Roles is not null);
    }
}
