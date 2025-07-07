namespace Application.Features.Attendance.Command.CreateAttendance;

public sealed class CreateAttendanceCommandValidator : AbstractValidator<CreateAttendanceCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateAttendanceCommandValidator(IServiceProvider serviceProvider)
    {
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        
        RuleFor(x => x.Attendance.Date)
            .NotEmpty().WithMessage("Please enter a date from the attendance date");
        
        RuleFor(x => x.Attendance.CheckInTime)
            .NotEmpty().WithMessage("Please enter a check-in time from the attendance date");
        
        RuleFor(x => x.Attendance.CheckOutTime)
            .NotEmpty().WithMessage("Please enter a check-out time from the attendance date");
        
        RuleFor(x => x.Attendance.Status)
            .NotEmpty().WithMessage("Please enter a status from the attendance date");

        RuleFor(x => x.Attendance)
            .Must(AreTimesValid);
    }
    
    
    private static bool AreTimesValid(AttendanceRequest attendanceRequest)
    {
        if (attendanceRequest.CheckInTime is null || attendanceRequest.CheckOutTime is null)
            return false;
        
        return attendanceRequest.CheckOutTime.Value > attendanceRequest.CheckInTime.Value && attendanceRequest.CheckInTime.Value < attendanceRequest.CheckOutTime.Value;
    }
}