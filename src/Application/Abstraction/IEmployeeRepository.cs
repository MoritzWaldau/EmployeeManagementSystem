namespace Application.Abstraction;

public interface IEmployeeRepository<TEntity> 
    : IBaseRepository<TEntity> where TEntity : Entity
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
}