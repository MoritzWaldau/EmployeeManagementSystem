namespace Tests.Integration;

[Collection("AspireApp")]
public class PayrollTests(AspireAppFixture fixture) : BaseTests(fixture)
{
        private readonly AspireAppFixture _fixture = fixture;

    [Fact]
    public async Task ApiShouldReturnPayrolls()
    {
        //Arrange
        var employee = await CreateEmployeeWithData();

        // Act
        var response = await _fixture.ApiClient.GetAsync(TestConfiguration.Payroll.GetAll);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonString = await response.Content.ReadAsStringAsync();
        var paginationResponse = JsonConvert.DeserializeObject<PaginationResponse<PayrollResponse>>(jsonString);
        Assert.NotNull(paginationResponse);
        Assert.NotEmpty(paginationResponse.Items);
    }
    
     [Fact]
    public async Task ApiShouldCreatePayroll()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(new PayrollRequest
            (
                employee.Id,
                2025,
                Month.August,
                9000,
                5678
            )), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await _fixture.ApiClient.PostAsync(TestConfiguration.Payroll.Create, httpContent);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ApiShouldReturnPayrollById()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var payroll = employee.Payrolls?.FirstOrDefault();
       
        // Act
        var response = await _fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Payroll.GetById, payroll?.Id));
       
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task ApiShouldUpdatePayroll()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var payroll = employee.Payrolls?.FirstOrDefault();
        
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(new PayrollRequest
            (
                employee.Id,
                2026,
                Month.September,
                20000,
                10000
            )), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await _fixture.ApiClient.PutAsync(string.Format(TestConfiguration.Payroll.Update, payroll?.Id), httpContent);
        var jsonString = await response.Content.ReadAsStringAsync();
        var updatedPayroll = JsonConvert.DeserializeObject<PayrollResponse>(jsonString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(updatedPayroll);
        Assert.Equal(2026, updatedPayroll.Year);
        Assert.Equal(Month.September, updatedPayroll.Month);
        Assert.Equal(20000, updatedPayroll.GrossSalary);   
        Assert.Equal(10000, updatedPayroll.NetSalary);
    }
    
    [Fact]
    public async Task ApiShouldDeletePayroll()
    {
        // Arrange
        var employee = await CreateEmployeeWithData();
        var payroll = employee.Payrolls?.FirstOrDefault();
        
        // Act
        var response = await _fixture.ApiClient.DeleteAsync(string.Format(TestConfiguration.Payroll.Delete, payroll?.Id));
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
        // Verify that the employee was deleted
        var getResponse = await _fixture.ApiClient.GetAsync(string.Format(TestConfiguration.Payroll.GetById, payroll?.Id));
        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);
    }
}