namespace Application.Abstraction.Repository;

public interface IBaseRepository<TEntity> where TEntity : IEntity
{
    Task<Result<TEntity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<TEntity>>> GetAllAsync(int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result<TEntity>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}