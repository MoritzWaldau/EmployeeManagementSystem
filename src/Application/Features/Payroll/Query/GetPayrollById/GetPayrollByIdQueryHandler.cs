namespace Application.Features.Payroll.Query.GetPayrollById;

public sealed class GetPayrollByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IQueryHandler<GetPayrollByIdQuery, GetPayrollByIdResponse>
{
    public async Task<GetPayrollByIdResponse> Handle(GetPayrollByIdQuery request, CancellationToken cancellationToken)
    {
        var payroll = await unitOfWork.Payrolls.GetByIdAsync(request.Id, cancellationToken);
        var mappedPayroll = mapper.Map<PayrollResponse>(payroll);
        return new GetPayrollByIdResponse(mappedPayroll);
    }
}