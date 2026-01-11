using Core.Application.Facades;
using Core.Application.Features.Users;
using Core.Application.Features.Auth;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Register Features
        services.AddScoped<CreateUser>();
        services.AddScoped<GetUser>();
        services.AddScoped<UpdateUser>();
        services.AddScoped<DeleteUser>();
        services.AddScoped<Login>();

        // Register Facades
        services.AddScoped<IUserFacade, UserFacade>();
        services.AddScoped<IAuthFacade, AuthFacade>();

        return services;
    }
}
