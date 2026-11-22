using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Api.Validation;

public static class InputValidation
{
    public static async Task<IActionResult?> CheckAsync<T>(
        IValidator<T> validator,
        T model,
        CancellationToken cancellationToken)
    {
        ValidationResult result = await validator.ValidateAsync(model, cancellationToken);
        if (result.IsValid)
            return null;

        var errors = new Dictionary<string, List<string>>();
        foreach (ValidationFailure failure in result.Errors)
        {
            if (!errors.TryGetValue(failure.PropertyName, out List<string>? messages))
            {
                messages = new List<string>();
                errors[failure.PropertyName] = messages;
            }

            messages.Add(failure.ErrorMessage);
        }

        var problem = new ValidationProblemDetails();
        foreach (KeyValuePair<string, List<string>> pair in errors)
            problem.Errors[pair.Key] = pair.Value.ToArray();

        problem.Status = StatusCodes.Status400BadRequest;
        problem.Title = "One or more validation errors occurred.";
        return new BadRequestObjectResult(problem);
    }
}
