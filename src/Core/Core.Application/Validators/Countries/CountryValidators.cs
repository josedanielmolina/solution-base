using Core.Application.DTOs.Countries;
using FluentValidation;

namespace Core.Application.Validators.Countries;

public class CreateCountryValidator : AbstractValidator<CreateCountryDto>
{
    public CreateCountryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido")
            .Length(2, 3).WithMessage("El código debe tener entre 2 y 3 caracteres")
            .Matches("^[A-Z]+$").WithMessage("El código debe contener solo letras mayúsculas");
    }
}

public class UpdateCountryValidator : AbstractValidator<UpdateCountryDto>
{
    public UpdateCountryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código es requerido")
            .Length(2, 3).WithMessage("El código debe tener entre 2 y 3 caracteres")
            .Matches("^[A-Z]+$").WithMessage("El código debe contener solo letras mayúsculas");
    }
}
