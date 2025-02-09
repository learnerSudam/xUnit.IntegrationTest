

using EmployeeAdminPortal.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EmployeeAdminPortal.IntegrationTests;

public class EmployeeAdminPortalWebApplicationFactory : WebApplicationFactory<Program>
{
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            var connectionString = GetConnectionString();
                services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    
                });
            });

        });
    }

    private static string? GetConnectionString()
        {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EmployeeAdminPortalWebApplicationFactory>()
            .Build();
        var connectionString = configuration.GetConnectionString("TestConnection");
        return connectionString;
    }

}