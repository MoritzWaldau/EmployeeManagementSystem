using Shared.Models.Attendance;
using Shared.Models.Payroll;

namespace Shared.Models.Employee;

public sealed record EmployeeResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime? ModifiedAt,
    bool IsActive,
    string FirstName,
    string LastName,
    string Email,
    IEnumerable<PayrollResponse>? Payrolls,
    IEnumerable<AttendanceResponse>? Attendances
) : BaseResponse(Id, CreatedAt, ModifiedAt, IsActive)
{
    public void Deconstruct(
        out string firstName,
        out string lastName,
        out string email,
        out bool isActive)
    {
        isActive = IsActive;
        firstName = FirstName;
        lastName = LastName;
        email = Email;
        
    }
}