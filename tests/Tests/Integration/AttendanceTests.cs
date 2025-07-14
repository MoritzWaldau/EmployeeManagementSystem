using Application.Models.Attendance;
using Application.Models.Pagination;

namespace Tests.Integration;

[Collection("AspireApp")]
public class AttendanceTests(AspireAppFixture fixture) : BaseTests(fixture)
{
    private readonly AspireAppFixture _fixture = fixture;

    [Fact]
    public async Task ApiShouldReturnAttendances()
    {
        //Arrange
        var employee = await CreateEmployeeWithData();

        // Act
        var response = await _fixture.ApiClient.GetAsync(TestConfiguration.Attendance.GetAll);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonString = await response.Content.ReadAsStringAsync();
        var paginationResponse = JsonConvert.DeserializeObject<PaginationResponse<AttendanceResponse>>(jsonString);
        Assert.NotNull(paginationResponse);
        Assert.NotEmpty(paginationResponse.Items);
    }
    
    [Fact]
    public async Task ApiShouldCreateAttendance()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(new AttendanceRequest
            {
                EmployeeId = employee.Id,
                CheckInTime = new TimeSpan(12, 0, 0),
                CheckOutTime = new TimeSpan(16, 0, 0),
                Date = new DateOnly(2025, 08, 17),
                Status = Status.WorkTime
            }), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await _fixture.ApiClient.PostAsync(TestConfiguration.Attendance.Create, httpContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ApiShouldReturnAttendanceById()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var attendance = employee.Attendances?.FirstOrDefault();
       
        // Act
        var response = await _fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Attendance.GetById, attendance?.Id));
       
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task ApiShouldUpdateAttendance()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var attendance = employee.Attendances?.FirstOrDefault();
        
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(new AttendanceRequest
            {
                EmployeeId = employee.Id,
                Date = attendance?.Date.AddDays(1),
            }), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await _fixture.ApiClient.PutAsync(string.Format(TestConfiguration.Attendance.Update, attendance?.Id), httpContent);
        var jsonString = await response.Content.ReadAsStringAsync();
        var updatedAttendance = JsonConvert.DeserializeObject<AttendanceResponse>(jsonString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(updatedAttendance);
        Assert.Equal(attendance?.Date.AddDays(1), updatedAttendance?.Date);
    }
    
    [Fact]
    public async Task ApiShouldDeleteAttendance()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var attendance = employee.Attendances?.FirstOrDefault();
        
        // Act
        var response = await _fixture.ApiClient.DeleteAsync(string.Format(TestConfiguration.Attendance.Delete, attendance?.Id));
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        // Verify that the employee was deleted
        var getResponse = await _fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Attendance.GetById, attendance?.Id));
        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);
    }
}
