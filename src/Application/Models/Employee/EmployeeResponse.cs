namespace Application.Models.Employee;

public sealed record EmployeeResponse : BaseResponse
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public IEnumerable<PayrollResponse>? Payrolls { get; init; }

}