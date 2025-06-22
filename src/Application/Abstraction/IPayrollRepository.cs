using Domain.Abstraction;

namespace Application.Abstraction;

public interface IPayrollRepository<TEntity> 
    : IBaseRepository<TEntity> where TEntity : Entity
{
    
}