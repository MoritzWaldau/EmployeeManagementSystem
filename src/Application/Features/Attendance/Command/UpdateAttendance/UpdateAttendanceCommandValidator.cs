namespace Application.Features.Attendance.Command.UpdateAttendance;

public class UpdateAttendanceCommandValidator : AbstractValidator<UpdateAttendanceCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateAttendanceCommandValidator(IServiceProvider serviceProvider)
    {
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        RuleFor(x => x)
            .MustAsync(AreTimesValidAsync);
    }

    private async Task<bool> AreTimesValidAsync(UpdateAttendanceCommand command, CancellationToken cancellationToken)
    {
        var attendance = command.Attendance;

        if (attendance.CheckInTime is null && attendance.CheckOutTime is null)
            return true;

        if (attendance.CheckInTime is not null && attendance.CheckOutTime is not null)
            return attendance.CheckOutTime > attendance.CheckInTime;

        var existingAttendance = await _unitOfWork.Attendances.GetByIdAsync(command.Id, cancellationToken);
        if (!existingAttendance.IsSuccess)
            return false;

        if (attendance.CheckInTime is not null)
            return existingAttendance.Value?.CheckOutTime > attendance.CheckInTime;

        if (attendance.CheckOutTime is not null)
            return existingAttendance.Value?.CheckInTime < attendance.CheckOutTime;

        return false;
    }
}