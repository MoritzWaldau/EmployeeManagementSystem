using Application.Behaviors.Validation;
using FluentValidation;

namespace Application.Features.Employee.Command.CreateEmployee;

public sealed class CreateEmployeeCommandValidator : BaseValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        RuleFor(x => x.Request.FirstName)
            .NotNull().NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");
        
        RuleFor(x => x.Request.LastName)
            .NotNull().NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");
        
        RuleFor(x => x.Request.Email)
            .NotNull().NotEmpty().WithMessage("E-Mail ist erforderlich.")
            .EmailAddress().WithMessage("Ungültiges E-Mail-Format.")
            .MustAsync(IsUniqueEmail).WithMessage("E-Mail existiert bereits.");
    }
}