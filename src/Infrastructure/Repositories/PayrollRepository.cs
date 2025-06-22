namespace Infrastructure.Repositories;

public sealed class PayrollRepository(DatabaseContext context) 
    : BaseRepository<Payroll>(context), IPayrollRepository<Payroll>
{
    
}