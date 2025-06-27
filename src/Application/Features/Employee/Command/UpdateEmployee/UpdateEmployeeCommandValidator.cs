namespace Application.Features.Employee.Command.UpdateEmployee;

public class UpdateEmployeeCommandValidator : BaseValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator(IServiceProvider provider) : base(provider)
    {
        RuleFor(x => x.Request.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Request.FirstName));
        
        RuleFor(x => x.Request.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Request.LastName));
        
        RuleFor(x => x.Request.Email)
            .NotEmpty().WithMessage("E-Mail ist erforderlich.")
            .EmailAddress().WithMessage("Ungültiges E-Mail-Format.")
            .MustAsync(IsUniqueEmail).WithMessage("E-Mail existiert bereits.")
            .When(x => !string.IsNullOrEmpty(x.Request.Email));
    }
}