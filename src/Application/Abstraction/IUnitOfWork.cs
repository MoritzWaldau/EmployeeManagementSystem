using Domain.Entities;

namespace Application.Abstraction;

public interface IUnitOfWork
{
    public IEmployeeRepository<Employee> Employees { get; }
    public IPayrollRepository<Payroll> Payrolls { get; }
}