namespace Tests.Integration;

[Collection("AspireApp")]
public class EmployeeTests(AspireAppFixture fixture)
{
    [Fact]
    public async Task ApiShouldReturnEmployees()
    {
        // Act
        var response = await fixture.ApiClient.GetAsync(TestConfiguration.Employee.GetAll);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
        var response = await fixture.ApiClient.PostAsync(TestConfiguration.Employee.Create, httpContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task ApiShouldReturnEmployeeById()
    {
        // Arrange
        var employee = await CreateEmployee();
        
        // Act
        var response = await fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Employee.GetById, employee.Id));
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task ApiShouldUpdateEmployee()
    {
        // Arrange
        var employee = await CreateEmployee();
        
        var httpContent = new StringContent(
        JsonConvert.SerializeObject(new EmployeeRequest
        {
            IsActive = false,
        }), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await fixture.ApiClient.PutAsync(string.Format(TestConfiguration.Employee.Update, employee.Id), httpContent);
        var jsonString = await response.Content.ReadAsStringAsync();
        var updatedEmployee = JsonConvert.DeserializeObject<EmployeeResponse>(jsonString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(updatedEmployee);
        Assert.False(updatedEmployee.IsActive);
    }
    
    [Fact]
    public async Task ApiShouldDeleteEmployee()
    {
        // Arrange
        var employee = await CreateEmployee();
        
        // Act
        var response = await fixture.ApiClient.DeleteAsync(string.Format(TestConfiguration.Employee.Delete, employee.Id));
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        // Verify that the employee was deleted
        var getResponse = await fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Employee.GetById, employee.Id));
        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);
    }

    private async Task<EmployeeResponse> CreateEmployee()
    {
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(new EmployeeRequest
            {
                FirstName = "Moritz",
                LastName = "Waldau",
                Email = $"{Guid.CreateVersion7()}@reply.de",
                IsActive = true,
            }), MediaTypeHeaderValue.Parse("application/json"));
        
        var res = await fixture.ApiClient.PostAsync(TestConfiguration.Employee.Create, httpContent);
        
        var jsonString = await res.Content.ReadAsStringAsync();
        var employee = JsonConvert.DeserializeObject<EmployeeResponse>(jsonString);
        
        return employee ?? throw new InvalidOperationException("Failed to create employee.");
    }
}