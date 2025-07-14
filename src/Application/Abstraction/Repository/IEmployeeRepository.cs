namespace Application.Abstraction.Repository;

public interface IEmployeeRepository
    : IBaseRepository<Employee> 
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
}