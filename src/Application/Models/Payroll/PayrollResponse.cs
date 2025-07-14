namespace Application.Models.Payroll;

public sealed record PayrollResponse : BaseResponse
{
    [JsonPropertyName("employeeId")]
    public required Guid EmployeeId { get; set; }
    
    [JsonPropertyName("year")]
    public required int Year { get; set; }
    
    [JsonPropertyName("month")]
    public required Month Month { get; set; }
    
    [JsonPropertyName("grossSalary")]
    public required double GrossSalary { get; set; }
    
    [JsonPropertyName("netSalary")]
    public required double NetSalary { get; set; }
}