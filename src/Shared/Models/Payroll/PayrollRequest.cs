namespace Shared.Models.Payroll;

public sealed record PayrollRequest(
    Guid? EmployeeId,
    int? Year,
    Month? Month,
    double? GrossSalary,
    double? NetSalary
);