using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Payroll : Entity
{
    public required Guid EmployeeId { get; set; }
    public required int Year { get; set; }
    public required Month Month { get; set; }
    public required double GrossSalary { get; set; }
    public required double NetSalary { get; set; }
    
    [ForeignKey("EmployeeId")]
    [InverseProperty("Payrolls")]
    public Employee? Employee { get; set; }
}