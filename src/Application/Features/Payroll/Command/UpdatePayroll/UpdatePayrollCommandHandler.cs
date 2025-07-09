namespace Application.Features.Payroll.Command.UpdatePayroll;

public class UpdatePayrollCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : ICommandHandler<UpdatePayrollCommand, Result<PayrollResponse>>
{
    public async Task<Result<PayrollResponse>> Handle(UpdatePayrollCommand request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.Payrolls.GetByIdAsync(request.Id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return Result<PayrollResponse>.Failure(result.ErrorMessage);
        }
        
        var entity = result.Value!;
        
        entity.EmployeeId = request.Request.EmployeeId ?? entity.EmployeeId;
        entity.Year = request.Request.Year ?? entity.Year;
        entity.Month = request.Request.Month ?? entity.Month;
        entity.GrossSalary = request.Request.GrossSalary ?? entity.GrossSalary;
        entity.NetSalary = request.Request.NetSalary ?? entity.NetSalary;
        
        var updateResult = await unitOfWork.Payrolls.UpdateAsync(entity, cancellationToken);

        if (!updateResult.IsSuccess)
        {
            return Result<PayrollResponse>.Failure(updateResult.ErrorMessage);
        }

        var mappedResponse = mapper.Map<PayrollResponse>(entity);
        await cache.RemoveByTagAsync(CacheTags.PayrollTag, cancellationToken: cancellationToken);
        return Result<PayrollResponse>.Success(mappedResponse);
    }
}