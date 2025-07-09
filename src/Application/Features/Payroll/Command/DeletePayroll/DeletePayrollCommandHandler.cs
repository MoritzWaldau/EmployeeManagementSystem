namespace Application.Features.Payroll.Command.DeletePayroll;

public sealed class DeletePayrollCommandHandler(IUnitOfWork unitOfWork, HybridCache cache) : ICommandHandler<DeletePayrollCommand, Result<PayrollResponse>>
{
    public async Task<Result<PayrollResponse>> Handle(DeletePayrollCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Payrolls.DeleteAsync(request.Id, cancellationToken);

        if (!result.IsSuccess) return Result<PayrollResponse>.Failure(result.ErrorMessage);

        await cache.RemoveByTagAsync(CacheTags.PayrollTag, cancellationToken);
        return Result<PayrollResponse>.Success();
    }
}