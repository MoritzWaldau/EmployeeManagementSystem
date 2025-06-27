namespace Application.Models.Payroll;

public sealed record PayrollRequest
{
    public Guid? EmployeeId { get; set; }
    public int? Year { get; set; }
    public Month? Month { get; set; }
    public double? GrossSalary { get; set; }
    public double? NetSalary { get; set; }
}