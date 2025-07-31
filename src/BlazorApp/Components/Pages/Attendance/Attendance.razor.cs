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
        await Api.DeleteEmployee(id);
        SnackbarService.Add("Attendance deleted", Severity.Success);
        await LoadAttendancesAsync(_currentPage, _currentPageSize);
    }
}