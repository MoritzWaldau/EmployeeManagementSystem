namespace Application.Features.Payroll.Query.GetPayrollById;

public sealed class GetPayrollByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetPayrollByIdQuery, Result<PayrollResponse>>
{
    public async Task<Result<PayrollResponse>> Handle(GetPayrollByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Payrolls.GetByIdAsync(request.Id, cancellationToken);
        if (!result.IsSuccess)
        {
            return Result<PayrollResponse>.Failure(result.ErrorMessage);
        }
        var payroll = result.Value!;
        var mappedPayroll = mapper.Map<PayrollResponse>(payroll);
        return Result<PayrollResponse>.Success(mappedPayroll);
    }
}