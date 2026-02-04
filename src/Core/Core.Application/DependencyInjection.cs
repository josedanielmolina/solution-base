using Core.Application.Features.Users;
using Core.Application.Features.Auth;
using Core.Application.Features.Roles;
using Core.Application.Features.Permissions;
using Core.Application.Features.Countries;
using Core.Application.Features.Cities;
using Core.Application.Features.Categories;
using Core.Application.Features.Establishments;
using Core.Application.Features.Events;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Register Auth Features
        services.AddScoped<Login>();
        services.AddScoped<ChangePassword>();
        services.AddScoped<RequestPasswordReset>();
        services.AddScoped<ResetPassword>();
        services.AddScoped<GetCurrentUser>();
        services.AddScoped<SetPassword>();

        // Register User Features
        services.AddScoped<CreateUser>();
        services.AddScoped<GetUser>();
        services.AddScoped<UpdateUser>();
        services.AddScoped<DeleteUser>();
        services.AddScoped<UpdateProfile>();
        services.AddScoped<AssignUserRoles>();
        services.AddScoped<SearchUsers>();

        // Register Role Features
        services.AddScoped<GetRoles>();
        services.AddScoped<GetRoleWithPermissions>();
        services.AddScoped<UpdateRolePermissions>();

        // Register Permission Features
        services.AddScoped<GetPermissions>();

        // Register Country Features
        services.AddScoped<IGetCountries, GetCountries>();
        services.AddScoped<IGetCountryById, GetCountryById>();
        services.AddScoped<ICreateCountry, CreateCountry>();
        services.AddScoped<IUpdateCountry, UpdateCountry>();
        services.AddScoped<IDeleteCountry, DeleteCountry>();

        // Register City Features
        services.AddScoped<IGetCities, GetCities>();
        services.AddScoped<IGetCitiesByCountry, GetCitiesByCountry>();
        services.AddScoped<IGetCityById, GetCityById>();
        services.AddScoped<ICreateCity, CreateCity>();
        services.AddScoped<IUpdateCity, UpdateCity>();
        services.AddScoped<IDeleteCity, DeleteCity>();

        // Register Category Features
        services.AddScoped<IGetCategories, GetCategories>();
        services.AddScoped<IGetCategoriesByCountry, GetCategoriesByCountry>();
        services.AddScoped<IGetCategoryById, GetCategoryById>();
        services.AddScoped<ICreateCategory, CreateCategory>();
        services.AddScoped<IUpdateCategory, UpdateCategory>();
        services.AddScoped<IDeleteCategory, DeleteCategory>();

        // Register Establishment Features
        services.AddScoped<IGetEstablishments, GetEstablishments>();
        services.AddScoped<IGetEstablishmentById, GetEstablishmentById>();
        services.AddScoped<ISearchEstablishments, SearchEstablishments>();
        services.AddScoped<ICreateEstablishment, CreateEstablishment>();
        services.AddScoped<IUpdateEstablishment, UpdateEstablishment>();
        services.AddScoped<IDeleteEstablishment, DeleteEstablishment>();
        services.AddScoped<IAddEstablishmentPhoto, AddEstablishmentPhoto>();
        services.AddScoped<IRemoveEstablishmentPhoto, RemoveEstablishmentPhoto>();
        services.AddScoped<ISetEstablishmentSchedules, SetEstablishmentSchedules>();

        // Register Court Features
        services.AddScoped<IGetCourtsByEstablishment, GetCourtsByEstablishment>();
        services.AddScoped<IGetCourtById, GetCourtById>();
        services.AddScoped<ICreateCourt, CreateCourt>();
        services.AddScoped<IUpdateCourt, UpdateCourt>();
        services.AddScoped<IDeleteCourt, DeleteCourt>();
        services.AddScoped<IAddCourtPhoto, AddCourtPhoto>();
        services.AddScoped<IRemoveCourtPhoto, RemoveCourtPhoto>();

        // Register Event Features (Phase 4)
        services.AddScoped<GetMyEventsFeature>();
        services.AddScoped<GetEventByPublicIdFeature>();
        services.AddScoped<CreateEventFeature>();
        services.AddScoped<UpdateEventFeature>();
        services.AddScoped<DeleteEventFeature>();
        services.AddScoped<UploadPosterFeature>();
        services.AddScoped<RemovePosterFeature>();
        services.AddScoped<UploadRulesPdfFeature>();
        services.AddScoped<RemoveRulesPdfFeature>();

        // Register Event Admin Features
        services.AddScoped<GetEventAdminsFeature>();
        services.AddScoped<InviteAdminFeature>();
        services.AddScoped<RemoveAdminFeature>();
        services.AddScoped<AcceptInvitationFeature>();
        services.AddScoped<GetPendingInvitationsFeature>();

        // Register Event Establishment Features
        services.AddScoped<GetEventEstablishmentsFeature>();
        services.AddScoped<AddEstablishmentFeature>();
        services.AddScoped<RemoveEstablishmentFeature>();
        services.AddScoped<SearchAvailableEstablishmentsFeature>();

        return services;
    }
}
