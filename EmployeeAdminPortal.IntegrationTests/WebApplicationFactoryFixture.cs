using EmployeeAdminPortal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeAdminPortal.IntegrationTests
{
    public class WebApplicationFactoryFixture : IClassFixture<EmployeeAdminPortalWebApplicationFactory>
    {
        public EmployeeAdminPortalWebApplicationFactory Factory { get; }
        public HttpClient Client { get; }
        public WebApplicationFactoryFixture()
        {
            Factory = new EmployeeAdminPortalWebApplicationFactory();

            // Apply Migrations (within the fixture constructor)
            using (var scope = Factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureDeleted(); // Or EnsureDeletedAsync()
                db.Database.Migrate();      // Or MigrateAsync()
            }

            Client = Factory.CreateClient(); // Create the client *after* migrations
        }
    }
}
