namespace Domain.Entities;

public sealed class Attendance : Entity
{
    public required Guid EmployeeId { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly CheckInTime { get; set; }
    public required TimeOnly CheckOutTime { get; set; }
    public required TimeSpan WorkDuration { get; set; }
    public required Status Status { get; set; }
    
    [ForeignKey("EmployeeId")]
    [InverseProperty("Attendances")]
    public Employee? Employee { get; set; }
}