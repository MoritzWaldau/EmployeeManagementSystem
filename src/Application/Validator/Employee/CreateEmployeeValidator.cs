namespace Application.Validator.Employee;

public class CreateEmployeeValidator : AbstractValidator<EmployeeRequest>
{
    private readonly IEmployeeRepository _employeeRepository;
    public CreateEmployeeValidator(IServiceProvider serviceProvider)
    {
        _employeeRepository = serviceProvider.GetRequiredService<IEmployeeRepository>();
        
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
    
    private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await _employeeRepository.IsEmailUniqueAsync(email, cancellationToken);
    }
}