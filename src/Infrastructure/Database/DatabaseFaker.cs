namespace Infrastructure.Database;

public class DatabaseFaker 
{
    private readonly Faker<Employee> _employeeFaker;
    private readonly Faker<Payroll> _payrollFaker;

    public DatabaseFaker()
    {
        _payrollFaker = new Faker<Payroll>()
            .RuleFor(x => x.GrossSalary, f => f.Finance.Random.Double(0, 100000))
            .RuleFor(x => x.NetSalary, (f, payroll) => payroll.GrossSalary - f.Finance.Random.Double(0, 20000))
            .RuleFor(x => x.Year, 2025)
            .RuleFor(x => x.Month, f => (Month)f.Random.Int(1, 12));

        _employeeFaker = new Faker<Employee>()
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.Email, f => f.Random.AlphaNumeric(15) + f.Internet.Email());
    }
    
    public List<Employee> GenerateEmployees(int count)
    {
        return _employeeFaker.Generate(count);
    }

    public List<Payroll> GeneratePayrolls(int count)
    {
        return _payrollFaker.Generate(count);

    }
}