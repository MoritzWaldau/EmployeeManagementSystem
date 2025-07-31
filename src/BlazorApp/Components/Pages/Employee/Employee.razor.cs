using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Models.Employee;

namespace BlazorApp.Components.Pages.Employee;

public partial class Employee : ComponentBase
{
    private int _totalPages;
    private int _currentPage = 1;
    private int _currentPageSize = 10;
    private IEnumerable<EmployeeResponse> _employees = [];
    
    protected override async Task OnInitializedAsync()
    {
        await LoadEmployeesAsync(_currentPage, _currentPageSize);
    }

    private async Task OnPageChanged(int page)
    {
        _currentPage = page;
        await LoadEmployeesAsync(_currentPage, _currentPageSize);
    }

    private async Task OnPageSizeChanged(int pageSize)
    {
        _currentPageSize = pageSize;
        await LoadEmployeesAsync(_currentPage, pageSize);
    }
    
    private async Task LoadEmployeesAsync(int page, int pageSize)
    {
        var result = await Api.GetEmployees(page, pageSize);
        _totalPages = result.PageCount;
        _currentPageSize = result.PageSize;
        _employees = result.Items;
    }
    
    private async Task OnRowClickEventHandler(TableRowClickEventArgs<EmployeeResponse> args)
    {
        if (args.Item is not null)
        {
            await ShowEmployeeModalAsync(args.Item);
        }
    }
    
    private async Task ShowEmployeeModalAsync(EmployeeResponse? employee)
    {
        var parameters = new DialogParameters<EmployeeModal>{{x => x.Employee, employee}};
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        
        var dialog = await DialogService.ShowAsync<EmployeeModal>(employee is not null ? "Edit employee" : "Create employee", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            var employeeRequest = (EmployeeRequest)result.Data!;
            SnackbarService.Add($"{employeeRequest.FirstName}, {employeeRequest.LastName}, {employeeRequest.Email}, {employeeRequest.IsActive}", Severity.Success);

            if (employee is not null)
            {
                await Api.UpdateEmployee(employee.Id, employeeRequest);
                SnackbarService.Add("Employee updated", Severity.Success);
            }
            else
            {
                await Api.CreateEmployee(employeeRequest);
                SnackbarService.Add("Employee created", Severity.Success);
            }

            // Refresh the employee list after updating
            await LoadEmployeesAsync(_currentPage, _currentPageSize);
        }
    }
    
    private async Task DeleteEmployeeAsync(Guid id)
    {
        var res = await DialogService.ShowMessageBox(
            "Delete employee",
            "Are you sure you want to delete this employee?",
            yesText: "Delete",
            cancelText: "Cancel"
        );
        
        if (res is not true)
            return;
        
        await Api.DeleteEmployee(id);
        SnackbarService.Add("Employee deleted", Severity.Success);
        await LoadEmployeesAsync(_currentPage, _currentPageSize);
    }
}