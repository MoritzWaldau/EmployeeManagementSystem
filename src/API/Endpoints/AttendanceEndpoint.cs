using API.Extensions;
using Application.Features.Attendance.Command.CreateAttendance;
using Application.Features.Attendance.Command.DeleteAttendance;
using Application.Features.Attendance.Command.UpdateAttendance;
using Application.Features.Attendance.Query.GetAllAttendance;
using Application.Features.Attendance.Query.GetAttendanceById;
using Application.Models.Attendance;

namespace API.Endpoints;

public sealed class AttendanceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/attendance");
        
        group.MapGet("/", GetAllAttendance)
            .WithSummary("Get all attendances")
            .WithName(nameof(GetAllAttendance))
            .Produces<PaginationResponse<AttendanceResponse>>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/{id}", GetAttendanceById)
            .WithSummary("Get attendance by id")
            .WithName(nameof(GetAttendanceById))
            .Produces<AttendanceResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPost("/", CreateAttendance)
            .WithSummary("Create attendance")
            .WithName(nameof(CreateAttendance))
            .Accepts<EmployeeRequest>("application/json")
            .Produces<EmployeeResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapPut("/{id}", UpdateAttendance)
            .WithSummary("Update attendance")
            .WithName(nameof(UpdateAttendance))
            .Accepts<AttendanceRequest>("application/json")
            .Produces<AttendanceResponse>()
            .Produces(StatusCodes.Status400BadRequest);
        
        group.MapDelete("/{id}", DeleteAttendance)
            .WithSummary("Delete attendance")
            .WithName(nameof(DeleteAttendance))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
        
    }
    
    private static async Task<IResult> GetAllAttendance([AsParameters] PaginationRequest request, ISender sender)
    {
        var result = await sender.Send(new GetAllAttendancesQuery(request));
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails("api/attendance", "Failed to get all attendances")));
    }
    
    private static async Task<IResult> GetAttendanceById(ISender sender, Guid id)
    {
        var result = await sender.Send(new GetAttendanceByIdQuery(id));
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/attendance/{id}", "Failed to get attendance by id")));
    }
    
    private static async Task<IResult> CreateAttendance(ISender sender, AttendanceRequest request)
    {
        var result = await sender.Send(new CreateAttendanceCommand(request));
        return result.Match(x => Results.Created(
                $"/api/attendance/{result.Value!.Id}", result.Value!), 
            err => Results.BadRequest(err.ToProblemDetails($"/api/attendance/{result.Value!.Id}", "Failed to create attendance"))
        );
    }
    
    private static async Task<IResult> UpdateAttendance(Guid id, AttendanceRequest request, ISender sender)
    {
        var result = await sender.Send(new UpdateAttendanceCommand(id, request));
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/attendance/{id}", "Failed to update attendance")));
    }
    
    private static async Task<IResult> DeleteAttendance(Guid id, ISender sender)
    {
        var result = await sender.Send(new DeleteAttendanceCommand(id));
        return result.Match(_ => Results.NoContent(), err => Results.BadRequest(err.ToProblemDetails($"api/attendance/{id}", "Failed to delete attendance")));
    }
}