namespace Application.Features.Payroll.Command.DeletePayroll;

public sealed class DeletePayrollCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeletePayrollCommand, Result<PayrollResponse>>
{
    public async Task<Result<PayrollResponse>> Handle(DeletePayrollCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Payrolls.DeleteAsync(request.Id, cancellationToken);
        return result.IsSuccess 
            ? Result<PayrollResponse>.Success() 
            : Result<PayrollResponse>.Failure(result.ErrorMessage);
    }
}