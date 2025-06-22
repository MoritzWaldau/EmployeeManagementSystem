using Application.Models.Employee;

namespace Application.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeResponse>();
        CreateMap<EmployeeRequest, Employee>();
    }
}