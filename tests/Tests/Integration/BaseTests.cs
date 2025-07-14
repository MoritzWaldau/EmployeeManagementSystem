using Application.Models.Attendance;
using Application.Models.Payroll;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;

namespace Tests.Integration;

public abstract class BaseTests(AspireAppFixture fixture)
{
    protected async Task<EmployeeResponse> CreateEmployeeWithData()
    {
        var httpContent = new StringContent(
        JsonConvert.SerializeObject(new EmployeeRequest
        {
            FirstName = "Moritz",
            LastName = "Waldau",
            Email = $"{Guid.CreateVersion7()}@reply.de",
            IsActive = true,
        }), MediaTypeHeaderValue.Parse("application/json"));

        var res = await fixture.ApiClient.PostAsync(TestConfiguration.Employee.Create, httpContent);

        var jsonString = await res.Content.ReadAsStringAsync();
        var employee = JsonConvert.DeserializeObject<EmployeeResponse>(jsonString);

        var attendanceRequest = new AttendanceRequest
        {
            EmployeeId = employee?.Id ?? throw new InvalidOperationException("Failed to create employee."),
            Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            CheckInTime = new TimeSpan(8, 0, 0), // 08:00 AM
            CheckOutTime = new TimeSpan(16, 0, 0), // 04:00 PM
            Status = Status.WorkTime
        };

        var attendanceContent = new StringContent(
            JsonConvert.SerializeObject(attendanceRequest), MediaTypeHeaderValue.Parse("application/json"));

        var attendanceResponse = await fixture.ApiClient.PostAsync(TestConfiguration.Attendance.Create, attendanceContent);
        
        jsonString = await attendanceResponse.Content.ReadAsStringAsync();
        var attendance = JsonConvert.DeserializeObject<AttendanceResponse>(jsonString) ?? throw new InvalidOperationException("Failed to deserialize attendance.");

        var payrollRequest = new PayrollRequest
        {
            EmployeeId = employee.Id,
            Month = Month.August,
            Year = DateTime.Now.Year,
            GrossSalary = 5000,
            NetSalary = 3000,
        };

        var payrollContent = new StringContent(
            JsonConvert.SerializeObject(payrollRequest), MediaTypeHeaderValue.Parse("application/json"));

        var payrollResponse = await fixture.ApiClient.PostAsync(TestConfiguration.Payroll.Create, payrollContent);

        jsonString = await payrollResponse.Content.ReadAsStringAsync();
        var payroll = JsonConvert.DeserializeObject<PayrollResponse>(jsonString) ?? throw new InvalidOperationException("Failed to create payroll.");


        return new EmployeeResponse
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            IsActive = employee.IsActive,
            Attendances = new List<AttendanceResponse> { attendance },
            Payrolls = new List<PayrollResponse> { payroll }
        };
    }
}
