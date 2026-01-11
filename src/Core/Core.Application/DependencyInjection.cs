using Core.Application.Facades;
using Core.Application.Operations.Users;
using Core.Application.Operations.Auth;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Register Operations
        services.AddScoped<CreateUserOperation>();
        services.AddScoped<GetUserOperation>();
        services.AddScoped<UpdateUserOperation>();
        services.AddScoped<DeleteUserOperation>();
        services.AddScoped<LoginOperation>();

        // Register Facades
        services.AddScoped<IUserFacade, UserFacade>();
        services.AddScoped<IAuthFacade, AuthFacade>();

        return services;
    }
}
