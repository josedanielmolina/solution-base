using Core.Application.Facades;
using Core.Application.Operations.Users;
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

        // Register Facades
        services.AddScoped<IUserFacade, UserFacade>();

        return services;
    }
}
