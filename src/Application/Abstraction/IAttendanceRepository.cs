namespace Application.Abstraction;

public interface IAttendanceRepository<TEntity> 
    : IBaseRepository<TEntity> where TEntity : Entity
{
    
}