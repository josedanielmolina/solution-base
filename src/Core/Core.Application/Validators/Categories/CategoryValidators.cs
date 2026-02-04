using Core.Application.DTOs.Categories;
using FluentValidation;

namespace Core.Application.Validators.Categories;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Gender)
            .InclusiveBetween(1, 3).WithMessage("El género debe ser válido (1=Masculino, 2=Femenino, 3=Mixto)");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país");
    }
}

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Gender)
            .InclusiveBetween(1, 3).WithMessage("El género debe ser válido (1=Masculino, 2=Femenino, 3=Mixto)");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país");
    }
}

