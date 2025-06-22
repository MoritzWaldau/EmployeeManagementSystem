namespace Infrastructure.Repositories;

public sealed class UnitOfWork(
    IEmployeeRepository<Employee> employeeRepository, 
    IPayrollRepository<Payroll> payrollRepository
    ) : IUnitOfWork
{
    public IEmployeeRepository<Employee> Employees => employeeRepository;
    public IPayrollRepository<Payroll> Payrolls => payrollRepository;
}