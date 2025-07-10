namespace Application.Validator.Employee;

public class CreateEmployeeValidator : BaseEmployeeValidator
{
    public CreateEmployeeValidator(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        RuleFor(x => x.FirstName)
            .NotNull().NotEmpty().WithMessage("Vorname is erforderlich.")
            .MaximumLength(50).WithMessage("Vorname darf nicht mehr als 50 Zeichen haben.");
        
        RuleFor(x => x.LastName)
            .NotNull().NotEmpty().WithMessage("Nachname is erforderlich.")
            .MaximumLength(50).WithMessage("Nachname darf nicht mehr als 50 Zeichen haben.");
        
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessage("E-Mail ist erforderlich.")
            .EmailAddress().WithMessage("Ungültiges E-Mail-Format.")
            .MustAsync(IsUniqueEmail).WithMessage("E-Mail existiert bereits.");
    }
}