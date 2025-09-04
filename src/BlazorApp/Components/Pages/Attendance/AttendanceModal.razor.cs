using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Enums;
using Shared.Models.Attendance;
using Shared.Models.Employee;

namespace BlazorApp.Components.Pages.Attendance;

public partial class AttendanceModal : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public AttendanceResponse? Attendance { get; set; }
    [Parameter] public Guid EmployeeId { get; set; }
    
    private DateTime? _date = DateTime.Now;
    private TimeSpan? _checkInTime;
    private TimeSpan? _checkOutTime;
    private TimeSpan? _workDuration;
    private Status _status;
    
    protected override void OnInitialized()
    {
        if (Attendance is null) return;
        _date = Attendance.Date.ToDateTime(TimeOnly.MinValue);
        _checkInTime = Attendance.CheckInTime;
        _checkOutTime = Attendance.CheckOutTime;
        _workDuration = Attendance.WorkDuration;
        _status = Attendance.Status;
    }
    
    private void Submit() => MudDialog.Close(DialogResult.Ok<AttendanceRequest>(new AttendanceRequest(
        EmployeeId,
        DateOnly.FromDateTime(_date!.Value),
        _checkInTime,
        _checkOutTime,
        _status
    )));

    private void Cancel() => MudDialog.Cancel();
}