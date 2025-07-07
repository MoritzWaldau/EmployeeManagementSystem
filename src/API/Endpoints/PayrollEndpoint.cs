using API.Extensions;

namespace API.Endpoints;

public sealed class PayrollEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
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
        
        group.MapPut("/{id}", UpdatePayroll)
            .WithSummary("Update payroll")
            .WithName(nameof(UpdatePayroll))
            .Accepts<PayrollRequest>("application/json")
            .Produces<PayrollResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{id}", DeletePayroll)
            .WithSummary("Delete payroll")
            .WithName(nameof(DeletePayroll))
            .Produces(StatusCodes.Status204NoContent);
    }
    
    private static async Task<IResult> GetAllPayrolls([AsParameters] PaginationRequest request, ISender sender)
    {
        var result = await sender.Send(new GetAllPayrollsQuery(request));
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails("api/payroll", "Failed to get all payrolls")));

    }
    
    private static async Task<IResult> GetPayrollById(Guid id, ISender sender)
    {
        var result = await sender.Send(new GetPayrollByIdQuery(id));
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/payroll/{id}", "Failed to get payroll by id")));

    }
    
    private static async Task<IResult> CreatePayroll(PayrollRequest request, ISender sender)
    {
        var result = await sender.Send(new CreatePayrollCommand(request));
        return result.Match(x => Results.Created(
                $"/api/payroll/{result.Value!.Id}", result.Value!), 
            err => Results.BadRequest(err.ToProblemDetails($"/api/payroll/{result.Value!.Id}", "Failed to create payroll"))
        );
    }
    
    private static async Task<IResult> UpdatePayroll(Guid id, PayrollRequest request, ISender sender)
    {
        var result = await sender.Send(new UpdatePayrollCommand(id, request));
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/payroll/{id}", "Failed to update payroll")));
    }
    
    private static async Task<IResult> DeletePayroll(Guid id, ISender sender)
    {
        var result = await sender.Send(new DeletePayrollCommand(id));
        return result.Match(_ => Results.NoContent(), err => Results.BadRequest(err.ToProblemDetails($"api/payroll/{id}", "Failed to delete payroll")));
    }
}