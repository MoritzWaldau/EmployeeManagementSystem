namespace Infrastructure.Repositories;

public sealed class AttendanceRepository(DatabaseContext context) 
    : BaseRepository<Attendance>(context), IAttendanceRepository<Attendance>
{
    
}