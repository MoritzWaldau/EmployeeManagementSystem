

namespace API.Endpoints;

public static class PayrollEndpoint 
{
    public static void MapPayrollEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/payroll");
        
        group.MapGet("/", GetAllPayrolls)
            .WithSummary("Get all payrolls")
            .WithName(nameof(GetAllPayrolls))
            .Produces<PaginationResponse<PayrollResponse>>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/{id:guid}", GetPayrollById)
            .WithSummary("Get payroll by id")
            .WithName(nameof(GetPayrollById))
            .Produces<PayrollResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/", CreatePayroll)
            .WithSummary("Create payroll")
            .WithName(nameof(CreatePayroll))
            .Accepts<PayrollRequest>("application/json")
            .Produces<PayrollResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{id:guid}", UpdatePayroll)
            .WithSummary("Update payroll")
            .WithName(nameof(UpdatePayroll))
            .Accepts<PayrollRequest>("application/json")
            .Produces<PayrollResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{id:guid}", DeletePayroll)
            .WithSummary("Delete payroll")
            .WithName(nameof(DeletePayroll))
            .Produces(StatusCodes.Status204NoContent);
    }
    
    private static async Task<IResult> GetAllPayrolls([AsParameters] PaginationRequest request, [FromServices] IPayrollService payrollService)
    {
        var result = await payrollService.GetAllAsync(request);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails("api/payroll", "Failed to get all payrolls")));

    }
    
    private static async Task<IResult> GetPayrollById(Guid id, [FromServices] IPayrollService payrollService)
    {
        var result = await payrollService.GetByIdAsync(id);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/payroll/{id}", "Failed to get payroll by id")));

    }
    
    private static async Task<IResult> CreatePayroll(PayrollRequest request, [FromServices] IPayrollService payrollService)
    {
        var result = await payrollService.CreateAsync(request);
        return result.Match(x => Results.Created(
                $"/api/payroll/{result.Value!.Id}", result.Value!), 
            err => Results.BadRequest(err.ToProblemDetails($"/api/payroll", "Failed to create payroll"))
        );
    }
    
    private static async Task<IResult> UpdatePayroll(Guid id, PayrollRequest request, [FromServices] IPayrollService payrollService)
    {
        var result = await payrollService.UpdateAsync(id, request);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/payroll/{id}", "Failed to update payroll")));
    }
    
    private static async Task<IResult> DeletePayroll(Guid id, [FromServices] IPayrollService payrollService)
    {
        var result = await payrollService.DeleteAsync(id);
        return result.Match(_ => Results.NoContent(), err => Results.BadRequest(err.ToProblemDetails($"api/payroll/{id}", "Failed to delete payroll")));
    }
}