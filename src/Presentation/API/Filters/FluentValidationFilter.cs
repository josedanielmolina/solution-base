using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

/// <summary>
/// Filtro global que ejecuta validación automática con FluentValidation
/// para todos los DTOs que tengan un validator registrado.
/// </summary>
public class FluentValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            
            if (_serviceProvider.GetService(validatorType) is IValidator validator)
            {
                var validationContext = new ValidationContext<object>(argument);
                var result = await validator.ValidateAsync(validationContext);

                if (!result.IsValid)
                {
                    var errorResponse = new
                    {
                        error = "Validation.Failed",
                        message = "One or more validation errors occurred.",
                        details = result.Errors.Select(e => new 
                        { 
                            property = e.PropertyName, 
                            message = e.ErrorMessage 
                        })
                    };

                    context.Result = new BadRequestObjectResult(errorResponse);
                    return;
                }
            }
        }

        await next();
    }
}
