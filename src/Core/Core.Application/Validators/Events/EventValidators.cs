using Core.Application.DTOs.Events;
using FluentValidation;

namespace Core.Application.Validators.Events;

public class CreateEventValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

        // OrganizerId is optional - can be assigned later
        RuleFor(x => x.OrganizerId)
            .GreaterThan(0).WithMessage("El organizador debe ser un usuario válido")
            .When(x => x.OrganizerId.HasValue);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("La fecha de inicio es requerida");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("La fecha de fin es requerida")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("La fecha de fin debe ser igual o posterior a la fecha de inicio");

        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.ContactPhone));

        RuleFor(x => x.WhatsApp)
            .MaximumLength(50).WithMessage("WhatsApp no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.WhatsApp));

        RuleFor(x => x.Facebook)
            .MaximumLength(200).WithMessage("Facebook no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Facebook));

        RuleFor(x => x.Instagram)
            .MaximumLength(200).WithMessage("Instagram no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Instagram));
    }
}

public class UpdateEventValidator : AbstractValidator<UpdateEventDto>
{
    public UpdateEventValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("La fecha de inicio es requerida");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("La fecha de fin es requerida")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("La fecha de fin debe ser igual o posterior a la fecha de inicio");

        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.ContactPhone));

        RuleFor(x => x.WhatsApp)
            .MaximumLength(50).WithMessage("WhatsApp no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.WhatsApp));

        RuleFor(x => x.Facebook)
            .MaximumLength(200).WithMessage("Facebook no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Facebook));

        RuleFor(x => x.Instagram)
            .MaximumLength(200).WithMessage("Instagram no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Instagram));
    }
}

public class InviteAdminValidator : AbstractValidator<InviteAdminDto>
{
    public InviteAdminValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email no es válido")
            .MaximumLength(256).WithMessage("El email no puede exceder 256 caracteres");
    }
}

public class UploadPosterValidator : AbstractValidator<UploadPosterDto>
{
    private const int MaxSizeBytes = 5 * 1024 * 1024; // 5MB

    public UploadPosterValidator()
    {
        RuleFor(x => x.ImageData)
            .NotEmpty().WithMessage("La imagen es requerida")
            .Must(BeValidBase64Image).WithMessage("La imagen debe ser un formato válido (JPG, PNG)")
            .Must(BeWithinSizeLimit).WithMessage("La imagen no puede exceder 5MB");
    }

    private bool BeValidBase64Image(string data)
    {
        if (string.IsNullOrEmpty(data)) return false;
        return data.StartsWith("data:image/jpeg") || 
               data.StartsWith("data:image/jpg") || 
               data.StartsWith("data:image/png");
    }

    private bool BeWithinSizeLimit(string data)
    {
        if (string.IsNullOrEmpty(data)) return false;
        // Approximate base64 size (base64 is ~4/3 larger than original)
        var approximateSize = (data.Length * 3) / 4;
        return approximateSize <= MaxSizeBytes;
    }
}

public class UploadRulesPdfValidator : AbstractValidator<UploadRulesPdfDto>
{
    private const int MaxSizeBytes = 5 * 1024 * 1024; // 5MB

    public UploadRulesPdfValidator()
    {
        RuleFor(x => x.PdfData)
            .NotEmpty().WithMessage("El PDF es requerido")
            .Must(BeValidBase64Pdf).WithMessage("El archivo debe ser un PDF")
            .Must(BeWithinSizeLimit).WithMessage("El PDF no puede exceder 5MB");
    }

    private bool BeValidBase64Pdf(string data)
    {
        if (string.IsNullOrEmpty(data)) return false;
        return data.StartsWith("data:application/pdf");
    }

    private bool BeWithinSizeLimit(string data)
    {
        if (string.IsNullOrEmpty(data)) return false;
        var approximateSize = (data.Length * 3) / 4;
        return approximateSize <= MaxSizeBytes;
    }
}
