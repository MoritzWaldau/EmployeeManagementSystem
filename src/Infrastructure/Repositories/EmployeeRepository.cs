namespace Infrastructure.Repositories;

public sealed class EmployeeRepository(DatabaseContext context) : 
    BaseRepository<Employee>(context), IEmployeeRepository<Employee>
{
    private readonly DatabaseContext _context = context;

    public override async Task<List<Employee>> GetAllAsync(int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await _context.Employees.Include(x => x.Payrolls)
            .AsNoTracking()
            .OrderBy(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public override async Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Employees.
            Include(e => e.Payrolls)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Employee with ID {id} not found.");;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        return !await _context.Employees.AnyAsync(e => e.Email == email, cancellationToken);
    }
}