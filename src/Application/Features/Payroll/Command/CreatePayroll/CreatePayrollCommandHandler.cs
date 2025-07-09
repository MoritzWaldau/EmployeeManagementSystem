namespace Application.Features.Payroll.Command.CreatePayroll;

public sealed class CreatePayrollCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : ICommandHandler<CreatePayrollCommand, Result<PayrollResponse>>
{
    public async Task<Result<PayrollResponse>> Handle(CreatePayrollCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Domain.Entities.Payroll>(request.PayrollRequest);
        var result = await unitOfWork.Payrolls.CreateAsync(entity, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<PayrollResponse>.Failure(result.ErrorMessage);
        }
        var mappedPayroll = mapper.Map<PayrollResponse>(entity);
        await cache.RemoveByTagAsync(CacheTags.PayrollTag, cancellationToken);
        return Result<PayrollResponse>.Success(mappedPayroll);
    }
}