namespace Application.Validator.Attendance;

public sealed class CreateAttendanceValidator : AbstractValidator<AttendanceRequest>
{
    
    public CreateAttendanceValidator(IServiceProvider serviceProvider)
    {
        
        
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Please enter a date from the attendance date");
        
        RuleFor(x => x.CheckInTime)
            .NotEmpty().WithMessage("Please enter a check-in time from the attendance date");
        
        RuleFor(x => x.CheckOutTime)
            .NotEmpty().WithMessage("Please enter a check-out time from the attendance date");
        
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Please enter a status from the attendance date");

        RuleFor(x => x)
            .Must(AreTimesValid);
    }
    
    
    private static bool AreTimesValid(AttendanceRequest attendanceRequest)
    {
        if (attendanceRequest.CheckInTime is null || attendanceRequest.CheckOutTime is null)
            return false;
        return attendanceRequest.CheckOutTime.Value > attendanceRequest.CheckInTime.Value && attendanceRequest.CheckInTime.Value < attendanceRequest.CheckOutTime.Value;
    }
}