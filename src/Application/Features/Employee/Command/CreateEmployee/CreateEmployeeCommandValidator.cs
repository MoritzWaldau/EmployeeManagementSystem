namespace Application.Features.Employee.Command.CreateEmployee;

public sealed class CreateEmployeeCommandValidator : BaseValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        RuleFor(x => x.Employee.FirstName)
            .NotNull().NotEmpty().WithMessage("Vorname is erforderlich.")
            .MaximumLength(50).WithMessage("Vorname darf nicht mehr als 50 Zeichen haben.");
        
        RuleFor(x => x.Employee.LastName)
            .NotNull().NotEmpty().WithMessage("Nachname is erforderlich.")
            .MaximumLength(50).WithMessage("Nachname darf nicht mehr als 50 Zeichen haben.");
        
        RuleFor(x => x.Employee.Email)
            .NotNull().NotEmpty().WithMessage("E-Mail ist erforderlich.")
            .EmailAddress().WithMessage("Ungültiges E-Mail-Format.")
            .MustAsync(IsUniqueEmail).WithMessage("E-Mail existiert bereits.");
    }
}