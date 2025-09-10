namespace GraphQL.Types;

[QueryType]
public static class PayrollQuery
{
    public static async Task<PayrollResponse> GetPayrollByIdAsync(
        Guid id,
        [Service] IPayrollService payrollService,
        CancellationToken cancellationToken = default)
    {
        var result = await payrollService.GetByIdAsync(id, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<PaginationResponse<PayrollResponse>> GetAllPayrollsAsync(
        PaginationRequest request,
        [Service] IPayrollService payrollService,
        CancellationToken cancellationToken = default)
    {
        var result = await payrollService.GetAllAsync(request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }
}
