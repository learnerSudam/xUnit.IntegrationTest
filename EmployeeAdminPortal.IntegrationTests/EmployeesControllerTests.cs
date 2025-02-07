using FluentAssertions;

namespace EmployeeAdminPortal.IntegrationTests
{
    public class EmployeesControllerTests : IClassFixture<EmployeeAdminPortalWebApplicationFactory>
    {


        public EmployeesControllerTests()
        {
            
        }
        [Fact]
        public async Task GetEmployees_Request_responds_with_OK()
        {

            // Arrange
            var application = new EmployeeAdminPortalWebApplicationFactory();
            var _client = application.CreateClient();
            // Act
            var response = await _client.GetAsync("api/employees");

            // Assert
            response.EnsureSuccessStatusCode(); // Asserts that the status code is 200 (OK)
            var getAllEmployeesResponse = await response.Content.ReadAsStringAsync();
            getAllEmployeesResponse?.Should().NotBeNullOrEmpty();
        }
    }
}

