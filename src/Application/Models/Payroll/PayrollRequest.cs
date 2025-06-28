namespace Application.Models.Payroll;

public sealed record PayrollRequest
{
    [JsonPropertyName("employeeId")]
    public Guid? EmployeeId { get; set; }
    
    [JsonPropertyName("year")]
    public int? Year { get; set; }
    
    [JsonPropertyName("month")]
    public Month? Month { get; set; }
    
    [JsonPropertyName("grossSalary")]
    public double? GrossSalary { get; set; }
    
    [JsonPropertyName("netSalary")]
    public double? NetSalary { get; set; }
}