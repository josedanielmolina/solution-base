using Core.Application.DTOs.Establishments;
using FluentValidation;

namespace Core.Application.Validators.Establishments;

public class CreateEstablishmentValidator : AbstractValidator<CreateEstablishmentDto>
{
    public CreateEstablishmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país");

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("Debe seleccionar una ciudad");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres");

        RuleFor(x => x.GoogleMapsUrl)
            .MaximumLength(500).WithMessage("La URL de Google Maps no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.GoogleMapsUrl));

        RuleFor(x => x.PhoneLandline)
            .MaximumLength(20).WithMessage("El teléfono fijo no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.PhoneLandline));

        RuleFor(x => x.PhoneMobile)
            .MaximumLength(20).WithMessage("El teléfono móvil no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.PhoneMobile));

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.PhoneLandline) || !string.IsNullOrEmpty(x.PhoneMobile))
            .WithMessage("Debe proporcionar al menos un teléfono (fijo o móvil)");

        RuleFor(x => x.ScheduleType)
            .InclusiveBetween(1, 2).WithMessage("El tipo de horario debe ser válido (1=Continuo, 2=Bloques)");
    }
}

public class UpdateEstablishmentValidator : AbstractValidator<UpdateEstablishmentDto>
{
    public UpdateEstablishmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país");

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("Debe seleccionar una ciudad");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(500).WithMessage("La dirección no puede exceder 500 caracteres");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.PhoneLandline) || !string.IsNullOrEmpty(x.PhoneMobile))
            .WithMessage("Debe proporcionar al menos un teléfono (fijo o móvil)");

        RuleFor(x => x.ScheduleType)
            .InclusiveBetween(1, 2).WithMessage("El tipo de horario debe ser válido (1=Continuo, 2=Bloques)");
    }
}

public class CreateCourtValidator : AbstractValidator<CreateCourtDto>
{
    public CreateCourtValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la cancha es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.CourtType)
            .InclusiveBetween(1, 2).WithMessage("El tipo de cancha debe ser válido (1=Indoor, 2=Outdoor)");
    }
}

public class UpdateCourtValidator : AbstractValidator<UpdateCourtDto>
{
    public UpdateCourtValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la cancha es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.CourtType)
            .InclusiveBetween(1, 2).WithMessage("El tipo de cancha debe ser válido (1=Indoor, 2=Outdoor)");
    }
}

public class CreatePhotoValidator : AbstractValidator<CreatePhotoDto>
{
    public CreatePhotoValidator()
    {
        RuleFor(x => x.ImageData)
            .NotEmpty().WithMessage("La imagen es requerida");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("El orden debe ser mayor o igual a 0");
    }
}
