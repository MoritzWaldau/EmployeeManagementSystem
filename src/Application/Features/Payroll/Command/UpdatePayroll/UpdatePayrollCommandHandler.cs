namespace Application.Features.Payroll.Command.UpdatePayroll;

public class UpdatePayrollCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : ICommandHandler<UpdatePayrollCommand, UpdatePayrollCommandResponse>
{
    public async Task<UpdatePayrollCommandResponse> Handle(UpdatePayrollCommand request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.Payrolls.GetByIdAsync(request.Id, cancellationToken);
        
        entity.EmployeeId = request.Request.EmployeeId ?? entity.EmployeeId;
        entity.Year = request.Request.Year ?? entity.Year;
        entity.Month = request.Request.Month ?? entity.Month;
        entity.GrossSalary = request.Request.GrossSalary ?? entity.GrossSalary;
        entity.NetSalary = request.Request.NetSalary ?? entity.NetSalary;
        
        await unitOfWork.Payrolls.UpdateAsync(entity, cancellationToken);
        var mappedResponse = mapper.Map<PayrollResponse>(entity);
        return new UpdatePayrollCommandResponse(mappedResponse);
    }
}