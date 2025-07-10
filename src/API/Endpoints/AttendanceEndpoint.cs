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
    
    private static async Task<IResult> GetAllAttendance([AsParameters] PaginationRequest request, IAttendanceService attendanceService)
    {
        var result = await attendanceService.GetAllAsync(request);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails("api/attendance", "Failed to get all attendances")));
    }
    
    private static async Task<IResult> GetAttendanceById(Guid id, IAttendanceService attendanceService)
    {
        var result = await attendanceService.GetByIdAsync(id);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/attendance/{id}", "Failed to get attendance by id")));
    }
    
    private static async Task<IResult> CreateAttendance(AttendanceRequest request, IAttendanceService attendanceService)
    {
        var result = await attendanceService.CreateAsync(request);
        return result.Match(x => Results.Created(
                $"/api/attendance/{result.Value!.Id}", result.Value!), 
            err => Results.BadRequest(err.ToProblemDetails($"/api/attendance", "Failed to create attendance"))
        );
    }
    
    private static async Task<IResult> UpdateAttendance(Guid id, AttendanceRequest request, IAttendanceService attendanceService)
    {
        var result = await attendanceService.UpdateAsync(id, request);
        return result.Match(Results.Ok, err => Results.BadRequest(err.ToProblemDetails($"api/attendance/{id}", "Failed to update attendance")));
    }
    
    private static async Task<IResult> DeleteAttendance(Guid id, IAttendanceService attendanceService)
    {
        var result = await attendanceService.DeleteAsync(id);
        return result.Match(_ => Results.NoContent(), err => Results.BadRequest(err.ToProblemDetails($"api/attendance/{id}", "Failed to delete attendance")));
    }
}