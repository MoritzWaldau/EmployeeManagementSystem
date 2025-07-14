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
            {
                EmployeeId = employee.Id,
                Year = DateTime.Now.Year,
                Month = Month.August,
                GrossSalary = 5000,
                NetSalary = 4500
            }), MediaTypeHeaderValue.Parse("application/json"));
        
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
            {
                EmployeeId = employee.Id,
                Year = payroll?.Year + 1,
                Month = payroll?.Month != Month.January ? Month.January : Month.December,
                GrossSalary = payroll?.GrossSalary == 0 ? 1000 : payroll?.GrossSalary + 1000,
                NetSalary = payroll?.NetSalary == 0 ? 1000 : payroll?.NetSalary + 1000
            }), MediaTypeHeaderValue.Parse("application/json"));
        
        // Act
        var response = await _fixture.ApiClient.PutAsync(string.Format(TestConfiguration.Payroll.Update, payroll?.Id), httpContent);
        var jsonString = await response.Content.ReadAsStringAsync();
        var updatedPayroll = JsonConvert.DeserializeObject<PayrollResponse>(jsonString);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(updatedPayroll);
        Assert.Equal(payroll?.Year + 1, updatedPayroll.Year);
        Assert.Equal(payroll?.Month != Month.January ? Month.January : Month.December, updatedPayroll.Month);
        Assert.Equal(payroll?.GrossSalary == 0 ? 1000 : payroll?.GrossSalary + 1000, updatedPayroll.GrossSalary);   
        Assert.Equal(payroll?.NetSalary == 0 ? 1000 : payroll?.NetSalary + 1000, updatedPayroll.NetSalary);
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