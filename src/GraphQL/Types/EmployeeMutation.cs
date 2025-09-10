using Application.Abstraction.Service;


namespace GraphQL.Types;

[MutationType]
public static class EmployeeMutation
{
    public static async Task<EmployeeResponse> CreateEmployeeAsync(
        EmployeeRequest request,
        [Service] IEmployeeService employeeService,
        CancellationToken cancellationToken = default)
    {
        var result = await employeeService.CreateAsync(request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<EmployeeResponse> UpdateEmployeeAsync(
        Guid id,
        EmployeeRequest request,
        [Service] IEmployeeService employeeService,
        CancellationToken cancellationToken = default)
    {
        var result = await employeeService.UpdateAsync(id, request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<EmployeeResponse> DeleteEmployeeAsync(
        Guid id,
        [Service] IEmployeeService employeeService,
        CancellationToken cancellationToken = default)
    {
        var result = await employeeService.DeleteAsync(id, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }
}