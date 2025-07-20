namespace Tests.Integration;

public abstract class BaseTests(AspireAppFixture fixture)
{
    protected async Task<EmployeeResponse> CreateEmployeeWithData()
    {
        var httpContent = new StringContent(
        JsonConvert.SerializeObject(new EmployeeRequest
        (
            "Moritz",
            "Waldau",
            $"{Guid.CreateVersion7()}@reply.de",
            true
        )), MediaTypeHeaderValue.Parse("application/json"));

        var res = await fixture.ApiClient.PostAsync(TestConfiguration.Employee.Create, httpContent);

        var jsonString = await res.Content.ReadAsStringAsync();
        var employee = JsonConvert.DeserializeObject<EmployeeResponse>(jsonString);

        var attendanceRequest = new AttendanceRequest
        (
            employee?.Id ?? throw new InvalidOperationException("Failed to create employee."),
            new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            new TimeSpan(8, 0, 0), // 08:00 AM
            new TimeSpan(16, 0, 0), // 04:00 PM
            Status.WorkTime
                
        );

        var attendanceContent = new StringContent(
            JsonConvert.SerializeObject(attendanceRequest), MediaTypeHeaderValue.Parse("application/json"));

        var attendanceResponse = await fixture.ApiClient.PostAsync(TestConfiguration.Attendance.Create, attendanceContent);
        
        jsonString = await attendanceResponse.Content.ReadAsStringAsync();
        var attendance = JsonConvert.DeserializeObject<AttendanceResponse>(jsonString) ?? throw new InvalidOperationException("Failed to deserialize attendance.");

        var payrollRequest = new PayrollRequest
            (
                employee?.Id,
                2025,
                Month.August,
                9000,
                5678
            );
        

        var payrollContent = new StringContent(
            JsonConvert.SerializeObject(payrollRequest), MediaTypeHeaderValue.Parse("application/json"));

        var payrollResponse = await fixture.ApiClient.PostAsync(TestConfiguration.Payroll.Create, payrollContent);

        jsonString = await payrollResponse.Content.ReadAsStringAsync();
        var payroll = JsonConvert.DeserializeObject<PayrollResponse>(jsonString) ?? throw new InvalidOperationException("Failed to create payroll.");


        return new EmployeeResponse
        (
            employee!.Id,
            employee.CreatedAt,
            employee.ModifiedAt,
            employee.IsActive,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            new List<PayrollResponse> { payroll },
            new List<AttendanceResponse> { attendance }
        );
    }
}
