using Application.Features.Payroll.Command.CreatePayroll;
using Application.Features.Payroll.Command.DeletePayroll;
using Application.Features.Payroll.Command.UpdatePayroll;
using Application.Features.Payroll.Query.GetAllPayrolls;
using Application.Features.Payroll.Query.GetPayrollById;
using Application.Models.Pagination;
using Application.Models.Payroll;

namespace API.Endpoints;

public sealed class PayrollEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/payroll");
        
        group.MapGet("/", GetAllPayrolls)
            .WithSummary("Get All Payrolls")
            .WithName(nameof(GetAllPayrolls))
            .Produces<PaginationResponse<PayrollResponse>>();
        
        group.MapGet("/{id}", GetPayrollById)
            .WithSummary("Get Payroll By Id")
            .WithName(nameof(GetPayrollById))
            .Produces<PayrollResponse>();
        
        group.MapPost("/", CreatePayroll)
            .WithSummary("Create Payroll")
            .WithName(nameof(CreatePayroll))
            .Accepts<PayrollRequest>("application/json")
            .Produces<PayrollResponse>(StatusCodes.Status201Created);
        
        group.MapPut("/{id}", UpdatePayroll)
            .WithSummary("Update Payroll")
            .WithName(nameof(UpdatePayroll))
            .Accepts<PayrollRequest>("application/json")
            .Produces<PayrollResponse>();
        
        group.MapDelete("/{id}", DeletePayroll)
            .WithSummary("Delete Payroll")
            .WithName(nameof(DeletePayroll))
            .Produces(StatusCodes.Status204NoContent);
    }
    
    private static async Task<IResult> GetAllPayrolls([AsParameters] PaginationRequest request, ISender sender)
    {
        var result = await sender.Send(new GetAllPayrollsQuery(request));
        return Results.Ok(result.Response);
    }
    
    private static async Task<IResult> GetPayrollById(Guid id, ISender sender)
    {
        var result = await sender.Send(new GetPayrollByIdQuery(id));
        return Results.Ok(result.Response);
    }
    
    private static async Task<IResult> CreatePayroll(PayrollRequest request, ISender sender)
    {
        var result = await sender.Send(new CreatePayrollCommand(request));
        return Results.Created($"/api/payroll/{result.Response.Id}", result.Response);
    }
    
    private static async Task<IResult> UpdatePayroll(Guid id, PayrollRequest request, ISender sender)
    {
        var result = await sender.Send(new UpdatePayrollCommand(id, request));
        return Results.Ok(result.Response);
    }
    
    private static async Task<IResult> DeletePayroll(Guid id, ISender sender)
    {
        await sender.Send(new DeletePayrollCommand(id));
        return Results.NoContent();
    }
}