namespace Infrastructure.Database;

public class DatabaseFaker 
{
    private static readonly DatabaseFaker Instance = new DatabaseFaker();
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
    
    public static List<Employee> GenerateSampleData(int count)
    {

        var employees = Instance._employeeFaker.Generate(count);

        foreach (var employee in employees)
        {
            employee.Payrolls = Instance._payrollFaker.Generate(3);

            foreach (var payroll in employee.Payrolls)
            {
                payroll.EmployeeId = employee.Id;
            }

            employee.Attendances = Instance._attendanceFaker.Generate(10);

            foreach (var attendance in employee.Attendances)
            {
                attendance.EmployeeId = employee.Id;
                attendance.WorkDuration = attendance.CheckOutTime - attendance.CheckInTime;
            }
        }

        return employees;
    }

    private static Payroll GenerateTestPayroll()
    {
        return Instance._payrollFaker.Generate(1).First();
    }
    
    private static Attendance GenerateTestAttendance()
    {
        return Instance._attendanceFaker.Generate(1).First();
    }

    private static Employee GenerateTestEmployee()
    {
        return Instance._employeeFaker.Generate(1).First();
    }
    
    public static T GenerateTestEntity<T>() where T : Entity
    {
        return typeof(T).Name switch
        {
            nameof(Employee) => (T)(object)GenerateTestEmployee(),
            nameof(Payroll) => (T)(object)GenerateTestPayroll(),
            nameof(Attendance) => (T)(object)GenerateTestAttendance(),
            _ => throw new InvalidOperationException($"No faker defined for type {typeof(T).Name}")
        };
    }

}