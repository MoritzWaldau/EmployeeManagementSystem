using Shared.Models.Attendance;

namespace BlazorApp.Components.Pages.Attendance;

public static class AttendanceFunction
{
    public static Func<AttendanceResponse, object> SortById => x => x.Id;
    public static Func<AttendanceResponse, object> SortByDate => x => x.Date;
    public static Func<AttendanceResponse, object> SortByCheckInTime => x => x.CheckInTime;
    public static Func<AttendanceResponse, object> SortByCheckOutTime => x => x.CheckOutTime;
    public static Func<AttendanceResponse, object> SortByCheckWorkDuration => x => x.WorkDuration;
    public static Func<AttendanceResponse, object> SortByStatus => x => x.Status;
}