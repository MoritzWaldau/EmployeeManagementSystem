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
        var employeeId = await CreateEmployeeWithData();

        // Act
        var response = await _fixture.ApiClient.GetAsync(TestConfiguration.Attendance.GetAll);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonString = await response.Content.ReadAsStringAsync();
        var paginationResponse = JsonConvert.DeserializeObject<PaginationResponse<AttendanceResponse>>(jsonString);
        Assert.NotNull(paginationResponse);
        Assert.NotEmpty(paginationResponse.Items);
    }
}
