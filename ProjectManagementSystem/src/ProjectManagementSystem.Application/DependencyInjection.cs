using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Application.Services;

namespace ProjectManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(_ => { }, typeof(DependencyInjection));

        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        return services;
    }

    public static void AssertMappingsAreValid(this IServiceProvider services)
    {
        services.GetRequiredService<IConfigurationProvider>().AssertConfigurationIsValid();
    }
}
