namespace Application.Features.Payroll.Command.DeletePayroll;

public sealed class DeletePayrollCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeletePayrollCommand>
{
    public async Task<Unit> Handle(DeletePayrollCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.Payrolls.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}