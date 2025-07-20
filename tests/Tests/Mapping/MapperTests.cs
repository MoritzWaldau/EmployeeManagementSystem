namespace Tests.Mapping;

public class MapperTests : IDisposable
{
    private readonly IMapper _mapper;
    public MapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EmployeeProfile>();
            cfg.AddProfile<AttendanceProfile>();
            cfg.AddProfile<PayrollProfile>();
        });
        
        _mapper = config.CreateMapper();
    }
    
    [Fact]
    public void MapEmployeeToEmployeeResponse()
    {
        // Arrange
        var employee = DatabaseFaker.GenerateTestEntity<Employee>();

        // Act
        var employeeResponse = _mapper.Map<EmployeeResponse>(employee);

        // Assert
        Assert.NotNull(employeeResponse);
        Assert.Equal(employee.Id, employeeResponse.Id);
        Assert.Equal(employee.FirstName, employeeResponse.FirstName);
        Assert.Equal(employee.LastName, employeeResponse.LastName);
        Assert.Equal(employee.Email, employeeResponse.Email);
        Assert.Equal(employee.CreatedAt, employeeResponse.CreatedAt);
        Assert.Equal(employee.ModifiedAt, employeeResponse.ModifiedAt);
        Assert.Equal(employee.IsActive, employeeResponse.IsActive);
    }

    [Fact]
    public void MapEmployeeRequestToEmployee()
    {
        // Arrange
        var employeeRequest = new EmployeeRequest
        (
            "John",
            "Doe",
            "JohnDoe@reply.de",
            true
        );
        
        // Act
        var employee = _mapper.Map<Employee>(employeeRequest);
        
        // Assert
        Assert.NotNull(employee);
        Assert.Equal(employeeRequest.FirstName, employee.FirstName);
        Assert.Equal(employeeRequest.LastName, employee.LastName);
        Assert.Equal(employeeRequest.Email, employee.Email);
        Assert.Equal(employeeRequest.IsActive, employee.IsActive);
    }
    
    [Fact]
    public void MapAttendanceToAttendanceResponse()
    {
        // Arrange
        var attendance = DatabaseFaker.GenerateTestEntity<Attendance>();

        // Act
        var attendanceResponse = _mapper.Map<AttendanceResponse>(attendance);

        // Assert
        Assert.NotNull(attendanceResponse);
        Assert.Equal(attendance.Id, attendanceResponse.Id);
        Assert.Equal(attendance.CreatedAt, attendanceResponse.CreatedAt);
        Assert.Equal(attendance.ModifiedAt, attendanceResponse.ModifiedAt);
        Assert.Equal(attendance.IsActive, attendanceResponse.IsActive);
        Assert.Equal(attendance.Date, attendanceResponse.Date);
        Assert.Equal(attendance.CheckInTime, attendanceResponse.CheckInTime);
        Assert.Equal(attendance.CheckOutTime, attendanceResponse.CheckOutTime);
        Assert.Equal(attendance.Status, attendanceResponse.Status);
        Assert.Equal(attendance.WorkDuration, attendanceResponse.WorkDuration);
    }
    
    [Fact]
    public void MapAttendanceRequestToAttendance()
    {
        // Arrange
        var attendanceRequest = new AttendanceRequest
        (
            Guid.CreateVersion7(),
            DateOnly.Parse("2025-08-17"),
             new TimeSpan(08, 00 ,00),
            new TimeSpan(17,30 ,00),
            Status.WorkTime
        );
        
        // Act
        var attendance = _mapper.Map<Attendance>(attendanceRequest);
        
        // Assert
        Assert.NotNull(attendance);
        Assert.Equal(attendanceRequest.EmployeeId, attendance.EmployeeId);
        Assert.Equal(attendanceRequest.Date, attendance.Date);
        Assert.Equal(attendanceRequest.CheckInTime, attendance.CheckInTime);
        Assert.Equal(attendanceRequest.CheckOutTime, attendance.CheckOutTime);
        Assert.Equal(attendanceRequest.Status, attendance.Status);
        Assert.Equal(attendanceRequest.CheckOutTime - attendanceRequest.CheckInTime, attendance.WorkDuration);
    }
    
    [Fact]
    public void MapPayrollToPayrollResponse()
    {
        // Arrange
        var payroll = DatabaseFaker.GenerateTestEntity<Payroll>();

        // Act
        var payrollResponse = _mapper.Map<PayrollResponse>(payroll);

        // Assert
        Assert.NotNull(payrollResponse);
        Assert.Equal(payroll.Id, payrollResponse.Id);
        Assert.Equal(payroll.CreatedAt, payrollResponse.CreatedAt);
        Assert.Equal(payroll.ModifiedAt, payrollResponse.ModifiedAt);
        Assert.Equal(payroll.IsActive, payrollResponse.IsActive);
        Assert.Equal(payroll.Month, payrollResponse.Month);
        Assert.Equal(payroll.Year, payrollResponse.Year);
        Assert.Equal(payroll.GrossSalary, payrollResponse.GrossSalary);
        Assert.Equal(payroll.NetSalary, payrollResponse.NetSalary);
    }

    [Fact]
    public void MapPayrollRequestToPayroll()
    {
        // Arrange
        var payrollRequest = new PayrollRequest
        (
            Guid.CreateVersion7(),
            2025,
            Month.August,
            9000,
            5678
        );
        
        // Act
        var payroll = _mapper.Map<Payroll>(payrollRequest);
        
        // Assert
        Assert.NotNull(payroll);
        Assert.Equal(payrollRequest.EmployeeId, payroll.EmployeeId);
        Assert.Equal(payrollRequest.Month, payroll.Month);
        Assert.Equal(payrollRequest.Year, payroll.Year);
        Assert.Equal(payrollRequest.GrossSalary, payroll.GrossSalary);
        Assert.Equal(payrollRequest.NetSalary, payroll.NetSalary);
    }

    
    
    
    public void Dispose()
    {

    }
}