namespace Tests.Integration;

[Collection("AspireApp")]
public class IntegrationTest(AspireAppFixture fixture)
{
    [Fact]
     public async Task ApiShouldBeHealthy()
     {
         // Act
         var response = await fixture.ApiClient.GetAsync("/health");
         var content = await response.Content.ReadAsStringAsync();
         // Assert
         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
         Assert.Contains("Healthy", content, StringComparison.OrdinalIgnoreCase);
     }
}