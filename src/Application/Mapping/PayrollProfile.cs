namespace Application.Mapping;

public class PayrollProfile : Profile
{
    public PayrollProfile()
    {
        CreateMap<Payroll, PayrollResponse>();
        CreateMap<PayrollRequest, Payroll>();
    }
}