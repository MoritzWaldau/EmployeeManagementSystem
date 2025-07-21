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
) : BaseResponse(Id, CreatedAt, ModifiedAt, IsActive);