using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Models.Employee;

namespace BlazorApp.Components.Pages.Employee;

public partial class EmployeeModal : ComponentBase
{
    
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public EmployeeResponse? Employee { get; set; }
    
    private string? _firstName;
    private string? _lastName;
    private string? _email;
    private bool _isActive;

    protected override void OnInitialized()
    {
        Employee?.Deconstruct(out _firstName, out _lastName, out _email, out _isActive);
    }
    
    private void Submit() => MudDialog.Close(DialogResult.Ok<EmployeeRequest>(new EmployeeRequest(
        _firstName, 
        _lastName,
        _email,
        _isActive
        )));

    private void Cancel() => MudDialog.Cancel();
}