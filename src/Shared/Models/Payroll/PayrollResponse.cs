namespace Shared.Models.Payroll;

public sealed record PayrollResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime? ModifiedAt,
    bool IsActive,
    Guid EmployeeId,
    int Year,
    Month Month,
    double GrossSalary,
    double NetSalary
) : BaseResponse(Id, CreatedAt, ModifiedAt, IsActive);