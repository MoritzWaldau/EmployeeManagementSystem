using Application.Models.Attendance;

namespace Application.Mapping;

public class AttendanceProfile : Profile
{
    public AttendanceProfile()
    {
        CreateMap<Attendance, AttendanceResponse>();
        CreateMap<AttendanceRequest, Attendance>()
            .ForMember(x => x.WorkDuration, opt => opt.MapFrom(src => src.CheckOutTime - src.CheckInTime));
    }
}