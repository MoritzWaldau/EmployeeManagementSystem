namespace GraphQL.Types;

[QueryType]
public static class EmployeeQuery
{
    public static async Task<EmployeeResponse> GetByIdAsync(
        Guid id,
        [Service] IEmployeeService employeeService,
        CancellationToken cancellationToken = default)
    {
        var result = await employeeService.GetByIdAsync(id, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<PaginationResponse<EmployeeResponse>> GetAllAsync(
        PaginationRequest request,
        [Service] IEmployeeService employeeService,
        CancellationToken cancellationToken = default)
    {
        var result = await employeeService.GetAllAsync(request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }
}

