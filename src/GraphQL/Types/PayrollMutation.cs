namespace GraphQL.Types;

[MutationType]
public static class PayrollMutation
{
    public static async Task<PayrollResponse> CreatePayrollAsync(
        PayrollRequest request,
        [Service] IPayrollService payrollService,
        CancellationToken cancellationToken = default)
    {
        var result = await payrollService.CreateAsync(request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<PayrollResponse> UpdatePayrollAsync(
        Guid id,
        PayrollRequest request,
        [Service] IPayrollService payrollService,
        CancellationToken cancellationToken = default)
    {
        var result = await payrollService.UpdateAsync(id, request, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }

    public static async Task<PayrollResponse> DeletePayrollAsync(
        Guid id,
        [Service] IPayrollService payrollService,
        CancellationToken cancellationToken = default)
    {
        var result = await payrollService.DeleteAsync(id, cancellationToken);
        return result.Match(x => x, err => throw new Exception("Error"));
    }
}
