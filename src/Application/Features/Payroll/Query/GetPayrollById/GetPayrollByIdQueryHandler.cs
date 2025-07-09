namespace Application.Features.Payroll.Query.GetPayrollById;

public sealed class GetPayrollByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, HybridCache cache) 
    : IQueryHandler<GetPayrollByIdQuery, Result<PayrollResponse>>
{
    public async Task<Result<PayrollResponse>> Handle(GetPayrollByIdQuery request, CancellationToken cancellationToken)
    {
        var errorMessage = "";
        var mappedEntity = await cache.GetOrCreateAsync($"{CacheKeys.PayrollKey}{request.Id}", async ct =>
        {
            var resultFromDb = await unitOfWork.Payrolls.GetByIdAsync(request.Id, ct);
            if (resultFromDb is { IsSuccess: true, HasValue: true })
                return mapper.Map<PayrollResponse>(resultFromDb.Value);

            errorMessage = resultFromDb.ErrorMessage;
            return null;
        }, tags: [CacheTags.PayrollTag], cancellationToken: cancellationToken);

        return mappedEntity is null 
            ? Result<PayrollResponse>.Failure(errorMessage) 
            : Result<PayrollResponse>.Success(mappedEntity);
    }
}