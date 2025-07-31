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
    
    private DateOnly _date;
    private TimeSpan _checkInTime;
    private TimeSpan _checkOutTime;
    private TimeSpan _workDuration;
    private Status _status;
    
    protected override void OnInitialized()
    {
        Attendance?.Deconstruct(out _, out _, out _, out _, out _date , out _checkInTime, out _checkOutTime, out _workDuration, out _status);
    }
    
    private void Submit() => MudDialog.Close(DialogResult.Ok<AttendanceRequest>(new AttendanceRequest(
        EmployeeId,
        _date,
        _checkInTime,
        _checkOutTime,
        _status
    )));

    private void Cancel() => MudDialog.Cancel();
}