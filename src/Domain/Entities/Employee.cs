using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public sealed class Employee : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    
    [InverseProperty("Employee")]
    public ICollection<Payroll>? Payrolls { get; set; }
}