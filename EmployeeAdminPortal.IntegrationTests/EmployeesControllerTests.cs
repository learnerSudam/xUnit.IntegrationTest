using System.Net.Http.Json;
using FluentAssertions;
using EmployeeAdminPortal.Models.Entities;

namespace EmployeeAdminPortal.IntegrationTests
{
    public class CreateEmployeeRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public int Salary { get; set; }
}
    public class UpdateEmployeeRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public int Salary { get; set; }
}
    public class EmployeesControllerTests : IClassFixture<WebApplicationFactoryFixture>
    {
        private readonly HttpClient _client;
        public EmployeesControllerTests(WebApplicationFactoryFixture fixture)
        {
            _client = fixture.Client; // Use the Client property from the fixture
        }
        [Fact]
        public async Task GetEmployees_Request_responds_with_OK()
        {

            // Act
            var response = await _client.GetAsync("api/employees");

            // Assert
            response.EnsureSuccessStatusCode(); // Asserts that the status code is 200 (OK)
            var getAllEmployeesResponse = await response.Content.ReadAsStringAsync();
            getAllEmployeesResponse?.Should().NotBeNullOrEmpty();
        }
        [Fact]
        public async Task AddEmployees_Request_responds_with_OK()
        {
            //arange
            var request = new CreateEmployeeRequest { Name = "John", Email = "jhondoe@email.com", Phone = "+918021324563", Salary = 10000 };
            // Act
            var response = await _client.PostAsJsonAsync("api/employees", request);
            // Assert
            response.EnsureSuccessStatusCode(); // Asserts that the status code is 200 (OK)
        }
        [Fact]
        public async Task GetEmployees_Should_Return_Added_Employee()
        
        {
            //arrange
            var postRequest = new CreateEmployeeRequest { Name = "Jesica", Email = "jhondoe@email.com", Phone = "+918021324563", Salary = 10000 };
            var  postResponse = await _client.PostAsJsonAsync("api/employees", postRequest);
            postResponse.EnsureSuccessStatusCode();
            // Act
            var response = await _client.GetAsync("api/employees");
            // Assert
            response.EnsureSuccessStatusCode();
            var getAllEmployeesResponse = await response.Content.ReadFromJsonAsync<List<Employee>>();
            getAllEmployeesResponse.Should().NotBeNullOrEmpty();
            var employee = getAllEmployeesResponse[0];
            employee.Name.Should().Be("Jesica");
        }
        [Fact]
        public async Task GetEmployeeById_Should_Return_Added_Employee()
        
        {
            //arrange
            var postRequest = new CreateEmployeeRequest { Name = "Robert", Email = "jhondoe@email.com", Phone = "+918021324563", Salary = 10000 };
            var  postResponse = await _client.PostAsJsonAsync("api/employees", postRequest);
            postResponse.EnsureSuccessStatusCode();
            var getAllEmployees = await _client.GetAsync("api/employees");
            getAllEmployees.EnsureSuccessStatusCode();
            var getAllEmployeesResponse = await getAllEmployees.Content.ReadFromJsonAsync<List<Employee>>();
            getAllEmployeesResponse.Should().NotBeNullOrEmpty();
            var addedEmployeeId = getAllEmployeesResponse[3].id;
            // Act
            var response = await _client.GetAsync($"api/employees/{addedEmployeeId}");
            // Assert
            response.EnsureSuccessStatusCode();
            var getEmployeeByIdResponse = await response.Content.ReadFromJsonAsync<Employee>();
            getEmployeeByIdResponse.Should().NotBeNull();
            getEmployeeByIdResponse.Name.Should().Be("Robert");
        }
        [Fact]
        public async Task GetEmployees_Request_responds_with_NOTFOUND_For_Invalid_EmployeeId()
        {
            // Act
            var response = await _client.GetAsync("api/employees/12345678-1234-1234-1234-123456789012");
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }   
        [Fact]
        public async Task UpdateEmployees_Request_responds_with_OK_And_Updates_EmployeeData()
        {
            var posteRequest = new CreateEmployeeRequest { Name = "Robin", Email = "robin@email.com", Phone = "+918021324563", Salary = 10000 };
            var  postResponse = await _client.PostAsJsonAsync("api/employees", posteRequest);
            postResponse.EnsureSuccessStatusCode();
            var getAllEmployees = await _client.GetAsync("api/employees");
            getAllEmployees.EnsureSuccessStatusCode();
            var getAllEmployeesResponse = await getAllEmployees.Content.ReadFromJsonAsync<List<Employee>>();
            getAllEmployeesResponse.Should().NotBeNullOrEmpty();
            var addedEmployeeId = getAllEmployeesResponse[2].id;
            var updateRequest = new UpdateEmployeeRequest { Name = "Robin", Email = "robin@email.com", Phone = "+918021324564", Salary = 10000 };
            var response = await _client.PutAsJsonAsync($"api/employees/{addedEmployeeId}", updateRequest);
            // Assert
            response.EnsureSuccessStatusCode();
            var getEmployeeByIdResponse = await _client.GetAsync($"api/employees/{addedEmployeeId}");
            getEmployeeByIdResponse.EnsureSuccessStatusCode();
            var getEmployeeByIdResponseContent = await getEmployeeByIdResponse.Content.ReadFromJsonAsync<Employee>();
            getEmployeeByIdResponseContent.Phone.Should().Be("+918021324564");

        }
    }
}

