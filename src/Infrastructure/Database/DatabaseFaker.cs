namespace Infrastructure.Database;

public class DatabaseFaker 
{
    private readonly Faker<Employee> _employeeFaker;
    private readonly Faker<Payroll> _payrollFaker;
    private readonly Faker<Attendance> _attendanceFaker;

    public DatabaseFaker()
    {
        _payrollFaker = new Faker<Payroll>()
            .RuleFor(x => x.GrossSalary, f => f.Finance.Random.Double(5_000, 10_000))
            .RuleFor(x => x.NetSalary, (f, payroll) => payroll.GrossSalary - f.Finance.Random.Double(2_000, 4_000))
            .RuleFor(x => x.Year, 2025)
            .RuleFor(x => x.Month, f => (Month)f.Random.Int(1, 12))
            .RuleFor(x => x.CreatedAt, DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .RuleFor(x => x.ModifiedAt, DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .RuleFor(x => x.IsActive, true);

        _attendanceFaker = new Faker<Attendance>()
            .RuleFor(x => x.Date, f => new DateOnly(2025, f.Random.Int(1, 12), f.Random.Int(1, 28)))
            .RuleFor(x => x.Status, f => f.PickRandom<Status>())
            .RuleFor(x => x.CheckInTime, f => new TimeSpan(f.Random.Int(5, 8), f.Random.Int(0, 30), 0))
            .RuleFor(x => x.CheckOutTime, f => new TimeSpan(f.Random.Int(14, 19), f.Random.Int(0, 30), 0))
            .RuleFor(x => x.CreatedAt, DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .RuleFor(x => x.ModifiedAt, DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .RuleFor(x => x.IsActive, true);

        _employeeFaker = new Faker<Employee>()
            .RuleFor(x => x.Id, f => Guid.CreateVersion7())
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Email, f => f.Random.AlphaNumeric(15) + f.Internet.Email())
            .RuleFor(x => x.CreatedAt, DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .RuleFor(x => x.ModifiedAt, DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            .RuleFor(x => x.IsActive, true);
    }
    
    public List<Employee> GenerateEmployees(int count)
    {

        var employees = _employeeFaker.Generate(count);

        foreach (var employee in employees)
        {
            employee.Payrolls = _payrollFaker.Generate(3);

            foreach (var payroll in employee.Payrolls)
            {
                payroll.EmployeeId = employee.Id;
            }

            employee.Attendances = _attendanceFaker.Generate(10);

            foreach (var attendance in employee.Attendances)
            {
                attendance.EmployeeId = employee.Id;
                attendance.WorkDuration = attendance.CheckOutTime - attendance.CheckInTime;
            }
        }

        return employees;
    }

}