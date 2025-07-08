namespace Tests.Mapping;

public class MapperTests : BaseMapperTests
{
    public MapperTests()
    {
        var mapperConfig = new MapperConfiguration(x =>
        {
            x.AddProfile<EmployeeProfile>();
            x.AddProfile<AttendanceProfile>();
            x.AddProfile<PayrollProfile>();
        });

        TestMapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public void TestEmployeeMappingToDomainModel()
    {
        var employee = DatabaseFaker.GenerateTestEntity<Employee>();
        var mappedEmployee = TestMapper.Map<EmployeeResponse>(employee);
        Assert.NotNull(mappedEmployee);
        CheckEntityMapping(employee, mappedEmployee);
        Assert.Equal(employee.FirstName, mappedEmployee.FirstName);
        Assert.Equal(employee.LastName, mappedEmployee.LastName);
        Assert.Equal(employee.Email, mappedEmployee.Email);
    }
    
    [Fact]
    public void TestAttendanceMappingToDomainModel()
    {
        var attendance = DatabaseFaker.GenerateTestEntity<Attendance>();
        var mappedAttendance = TestMapper.Map<AttendanceResponse>(attendance);
        Assert.NotNull(mappedAttendance);
        CheckEntityMapping(attendance, mappedAttendance);
        Assert.Equal(attendance.Date, mappedAttendance.Date);
        Assert.Equal(attendance.Status, mappedAttendance.Status);
        Assert.Equal(attendance.CheckInTime, mappedAttendance.CheckInTime);
        Assert.Equal(attendance.CheckOutTime, mappedAttendance.CheckOutTime);
        
    }
    
    [Fact]
    public void TestPayrollMappingToDomainModel()
    {
        var payroll = DatabaseFaker.GenerateTestEntity<Payroll>();
        var mappedPayroll = TestMapper.Map<PayrollResponse>(payroll);
        Assert.NotNull(mappedPayroll);
        CheckEntityMapping(payroll, mappedPayroll);
        Assert.Equal(payroll.GrossSalary, mappedPayroll.GrossSalary);
        Assert.Equal(payroll.NetSalary, mappedPayroll.NetSalary);
        Assert.Equal(payroll.Year, mappedPayroll.Year);
        Assert.Equal(payroll.Month, mappedPayroll.Month);
    }
}
