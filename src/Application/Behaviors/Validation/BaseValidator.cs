namespace Application.Behaviors.Validation;

public abstract class BaseValidator<T>(IServiceProvider provider) : AbstractValidator<T>
{
    private IUnitOfWork UnitOfWork => provider.GetRequiredService<IUnitOfWork>();
    
    protected async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await UnitOfWork.Employees.IsEmailUniqueAsync(email, cancellationToken);
    }
}