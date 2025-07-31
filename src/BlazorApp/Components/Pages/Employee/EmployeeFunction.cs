using Shared.Models.Employee;

namespace BlazorApp.Components.Pages.Employee;

public static class EmployeeFunction
{
    public static Func<EmployeeResponse, object> SortById => x => x.Id;
    public static Func<EmployeeResponse, object> SortByFirstname => x => x.FirstName;
    public static Func<EmployeeResponse, object> SortByLastname => x => x.LastName;
    public static Func<EmployeeResponse, object> SortByEmail => x => x.Email;
}