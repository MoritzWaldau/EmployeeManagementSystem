namespace Infrastructure.Repositories;

public sealed class UnitOfWork(
    IEmployeeRepository<Employee> employeeRepository, 
    IPayrollRepository<Payroll> payrollRepository,
    IAttendanceRepository<Attendance> attendanceRepository
    ) : IUnitOfWork
{
    public IEmployeeRepository<Employee> Employees => employeeRepository;
    public IPayrollRepository<Payroll> Payrolls => payrollRepository;
    public IAttendanceRepository<Attendance> Attendances => attendanceRepository;
}