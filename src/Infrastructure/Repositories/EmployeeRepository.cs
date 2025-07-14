namespace Infrastructure.Repositories;

public sealed class EmployeeRepository(DatabaseContext context) : 
    BaseRepository<Employee>(context), IEmployeeRepository
{
    private readonly DatabaseContext _context = context;

    private IQueryable<Employee> EmployeeQuery => _context.Employees
        .Include(e => e.Payrolls)
        .Include(e => e.Attendances)
        .AsNoTracking();
    
    public override async Task<Result<List<Employee>>> GetAllAsync(int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var employees = await EmployeeQuery
                .OrderBy(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);
            
            return Result<List<Employee>>.Success(employees);
        }
        catch (Exception e)
        {
            return Result<List<Employee>>.Failure(e.Message);
        }
    }

    public override async Task<Result<Employee>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await EmployeeQuery
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        return entity != null ? 
                Result<Employee>.Success(entity) : 
                Result<Employee>.Failure($"Employee with ID {id} not found.");
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        return !await _context.Employees.AnyAsync(e => e.Email == email, cancellationToken);
    }
}