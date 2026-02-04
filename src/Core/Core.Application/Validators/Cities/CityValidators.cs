using Core.Application.DTOs.Cities;
using FluentValidation;

namespace Core.Application.Validators.Cities;

public class CreateCityValidator : AbstractValidator<CreateCityDto>
{
    public CreateCityValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país");
    }
}

public class UpdateCityValidator : AbstractValidator<UpdateCityDto>
{
    public UpdateCityValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país");
    }
}
