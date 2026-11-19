using ExpenseManager.Infrastructure;
using ExpenseManager.IOC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpenseManager.Tests.Integration.Utilities
{
    public class ExpenseManagerFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public ExpenseManagerFactory()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            _connectionString = _config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found.");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<Context>>();
                services.RemoveAll<Context>();

                services.InjectServices()
                        .InjectRepositories()
                        .InjectValidators()
                        .InjectSettings(_config)
                        .InjectExternalServices(_config)
                        .InjectCache(_config);

                services.AddDbContext<Context>(options =>
                    options.UseSqlServer(_connectionString));
            });
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            using var scope = Services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<Context>();

            await context.Database.MigrateAsync();
        }


        public async Task CleanupAsync()
        {
            using var scope = Services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<Context>();

            await context.Expenses.ExecuteDeleteAsync();
        }

        async Task IAsyncLifetime.DisposeAsync() => 
            await CleanupAsync();
    }
}
