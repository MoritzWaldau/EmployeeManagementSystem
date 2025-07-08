namespace Infrastructure.Repositories;

public class BaseRepository<TEntity>(DatabaseContext context) 
    : IBaseRepository<TEntity> where TEntity : Entity
{
    public virtual async Task<Result<TEntity>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ;
        return entity != null ? 
            Result<TEntity>.Success(entity) : 
            Result<TEntity>.Failure($"Entity with ID {id} not found.");
    }

    public virtual async Task<Result<List<TEntity>>> GetAllAsync(int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await context.Set<TEntity>()
                .AsNoTracking()
                .OrderBy(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);
            
            return Result<List<TEntity>>.Success(entities);
        }
        catch (Exception e)
        {
            return Result<List<TEntity>>.Failure(e.Message);
        }
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<TEntity>().CountAsync(cancellationToken);
    }

    public virtual async Task<Result> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
        
        return Result.Success();
    }

    public virtual async Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            context.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
        
        return Result.Success();
    }

    public async Task<Result<TEntity>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        
        if (!entity.IsSuccess)
        {
            return entity;
        }
        context.Remove(entity.Value!);
        await context.SaveChangesAsync(cancellationToken);
        
        return Result<TEntity>.Success();
    }
}