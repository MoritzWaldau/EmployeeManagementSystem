using BlazorApp.Components.Pages.Employee;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Models.Attendance;
using Shared.Models.Employee;

namespace BlazorApp.Components.Pages.Attendance;

public partial class Attendance : ComponentBase
{
    private int _totalPages;
    private int _currentPage = 1;
    private int _currentPageSize = 10;
    private IEnumerable<AttendanceResponse> _attendances = [];
    
    protected override async Task OnInitializedAsync()
    {
        await LoadAttendancesAsync(_currentPage, _currentPageSize);
    }
    
    private async Task OnPageChanged(int page)
    {
        _currentPage = page;
        await LoadAttendancesAsync(_currentPage, _currentPageSize);
    }

    private async Task OnPageSizeChanged(int pageSize)
    {
        _currentPageSize = pageSize;
        await LoadAttendancesAsync(_currentPage, pageSize);
    }
    
    private async Task LoadAttendancesAsync(int page, int pageSize)
    {
        var result = await Api.GetAttendances(page, pageSize);
        _totalPages = result.PageCount;
        _currentPageSize = result.PageSize;
        _attendances = result.Items;
    }
    
    private async Task DeleteAttendanceAsync(Guid id)
    {
        await Api.DeleteAttendance(id);
        SnackbarService.Add("Attendance deleted", Severity.Success);
        await LoadAttendancesAsync(_currentPage, _currentPageSize);
    }

    private async Task OnRowClickEventHandler(TableRowClickEventArgs<AttendanceResponse> args)
    {
        if (args.Item is not null)
        {
            await ShowAttendanceModalAsync(args.Item);
        }
    }
    
    private async Task ShowAttendanceModalAsync(AttendanceResponse? attendance)
    {
        var parameters = new DialogParameters<AttendanceModal> { { x => x.Attendance, attendance }, { x => x.EmployeeId, Guid.Empty } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        
        var dialog = await DialogService.ShowAsync<AttendanceModal>(attendance is not null ? "Edit attendance" : "Create attendance", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            var attendanceRequest = (AttendanceRequest)result.Data!;

            if (attendance is not null)
            {
                await Api.UpdateAttendance(attendance.Id, attendanceRequest);
                SnackbarService.Add("Attendance updated", Severity.Success);
            }
            else
            {
                await Api.CreateAttendance(attendanceRequest);
                SnackbarService.Add("Attendance created", Severity.Success);
            }

            await LoadAttendancesAsync(_currentPage, _currentPageSize);
        }
    }
}