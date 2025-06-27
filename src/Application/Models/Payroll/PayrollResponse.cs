namespace Application.Models.Payroll;

public sealed record PayrollResponse : BaseResponse
{
    public required Guid EmployeeId { get; set; }
    public required int Year { get; set; }
    public required Month Month { get; set; }
    public required double GrossSalary { get; set; }
    public required double NetSalary { get; set; }
}