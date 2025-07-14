using Application.Models.Pagination;

namespace Tests.Integration;

[Collection("AspireApp")]
public class EmployeeTests(AspireAppFixture fixture) : BaseTests(fixture)
{
    private readonly AspireAppFixture _fixture = fixture;

    [Fact]
    public async Task ApiShouldReturnEmployees()
    {
        //Arrange
        var employee = await CreateEmployeeWithData();

        // Act
        var response = await _fixture.ApiClient.GetAsync(TestConfiguration.Employee.GetAll);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonString = await response.Content.ReadAsStringAsync();
        var paginationResponse = JsonConvert.DeserializeObject<PaginationResponse<EmployeeResponse>>(jsonString);
        Assert.NotNull(paginationResponse);
        Assert.NotEmpty(paginationResponse.Items);
        Assert.Contains(paginationResponse.Items, e => e.Id == employee.Id);
    }
    
    [Fact]
    public async Task ApiShouldCreateEmployee()
    {
        // Arrange
        var httpContent = new StringContent(
        JsonConvert.SerializeObject(new EmployeeRequest
        {
            FirstName = "Moritz",
            LastName = "Waldau",
            Email = "m.waldau@reply.de",
            IsActive = true,
        }), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await _fixture.ApiClient.PostAsync(TestConfiguration.Employee.Create, httpContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task ApiShouldReturnEmployeeById()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        
        // Act
        var response = await _fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Employee.GetById, employee.Id));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task ApiShouldUpdateEmployee()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        
        var httpContent = new StringContent(
        JsonConvert.SerializeObject(new EmployeeRequest
        {
            FirstName = "Updated " + employee.FirstName ,
            LastName = "Updated " + employee.LastName,
            Email = "Updated " + employee.Email,
            IsActive = false,
        }), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await _fixture.ApiClient.PutAsync(string.Format(TestConfiguration.Employee.Update, employee.Id), httpContent);
        var jsonString = await response.Content.ReadAsStringAsync();
        var updatedEmployee = JsonConvert.DeserializeObject<EmployeeResponse>(jsonString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(updatedEmployee);
        Assert.Equal("Updated " + employee.FirstName, updatedEmployee.FirstName);
        Assert.Equal("Updated " + employee.LastName, updatedEmployee.LastName);
        Assert.Equal("Updated " + employee.Email, updatedEmployee.Email);
        Assert.False(updatedEmployee.IsActive);
    }
    
    [Fact]
    public async Task ApiShouldDeleteEmployee()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        
        // Act
        var response = await _fixture.ApiClient.DeleteAsync(string.Format(TestConfiguration.Employee.Delete, employee.Id));
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        // Verify that the employee was deleted
        var getResponse = await _fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Employee.GetById, employee.Id));
        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);
    }
}