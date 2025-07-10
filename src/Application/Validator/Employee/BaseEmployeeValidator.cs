namespace Application.Validator.Employee;

public abstract class BaseEmployeeValidator(IServiceProvider provider) : AbstractValidator<EmployeeRequest>
{
    private IUnitOfWork UnitOfWork => provider.GetRequiredService<IUnitOfWork>();
    
    protected async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await UnitOfWork.Employees.IsEmailUniqueAsync(email, cancellationToken);
    }
}