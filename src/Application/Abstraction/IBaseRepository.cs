using System.Linq.Expressions;
using Application.Models.Pagination;
using Domain.Abstraction;

namespace Application.Abstraction;

public interface IBaseRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllAsync(int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}