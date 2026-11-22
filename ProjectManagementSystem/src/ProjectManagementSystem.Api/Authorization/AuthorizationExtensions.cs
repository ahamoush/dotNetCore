using Microsoft.AspNetCore.Authorization;
using ProjectManagementSystem.Application.Authorization;

namespace ProjectManagementSystem.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPermissionPolicies(this IServiceCollection services)
    {
        RolePermissionMap.EnsureEveryPermissionHasARole();

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            foreach (var permission in AppPermissions.All)
            {
                var roles = RolePermissionMap.RolesWith(permission);
                options.AddPolicy(permission, policy => policy.RequireRole((IEnumerable<string>)roles));
            }
        });

        return services;
    }
}
