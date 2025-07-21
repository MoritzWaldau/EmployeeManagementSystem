using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shared.Models.Employee;

namespace BlazorApp.Components.Pages;

public partial class Employee : ComponentBase
{
    private bool _isLoading = true;
    private IEnumerable<EmployeeResponse> _employees = [];

    protected override async Task OnInitializedAsync()
    {
        var result = await Api.GetEmployees();
        _employees = result.Items;
        _isLoading = false;
    }
}