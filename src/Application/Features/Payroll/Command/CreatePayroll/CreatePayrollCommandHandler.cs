using Application.Models.Payroll;

namespace Application.Features.Payroll.Command.CreatePayroll;

public sealed class CreatePayrollCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : ICommandHandler<CreatePayrollCommand, CreatePayrollResponse>
{
    public async Task<CreatePayrollResponse> Handle(CreatePayrollCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Domain.Entities.Payroll>(request.PayrollRequest);
        await unitOfWork.Payrolls.CreateAsync(entity, cancellationToken);
        var mappedPayroll = mapper.Map<PayrollResponse>(entity);
        return new CreatePayrollResponse(mappedPayroll);
    }
}