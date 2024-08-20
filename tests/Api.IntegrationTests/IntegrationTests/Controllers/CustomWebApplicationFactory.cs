// using System.Data.Common;
// using Infrastructure.Data;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Data.Sqlite;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;

// public class CustomWebApplicationFactory<TProgram>
//     : WebApplicationFactory<TProgram> where TProgram : class
// {
//     protected override void ConfigureWebHost(IWebHostBuilder builder)
//     {
//         builder.ConfigureServices(services =>
//         {
//             var dbContextDescriptor = services.SingleOrDefault(
//                 d => d.ServiceType ==
//                     typeof(DbContextOptions<BlogDbContext>));

//             services.Remove(dbContextDescriptor);

//             var dbConnectionDescriptor = services.SingleOrDefault(
//                 d => d.ServiceType ==
//                     typeof(DbConnection));

//             services.Remove(dbConnectionDescriptor);

//             // Create open SqliteConnection so EF won't automatically close it.
//             services.AddSingleton<DbConnection>(container =>
//             {
//                 var connection = new SqliteConnection("DataSource=:memory:");
//                 connection.Open();

//                 return connection;
//             });

//             services.AddDbContext<BlogDbContext>((container, options) =>
//             {
//                 var connection = container.GetRequiredService<DbConnection>();
//                 options.UseSqlite(connection);
//             });


//                  var descriptor = services.SingleOrDefault(
//                 d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>));
            
//             if (descriptor != null)
//             {
//                 services.Remove(descriptor);
//             }

//             // Add the DbContext using an in-memory SQLite database
//             services.AddDbContext<BlogDbContext>(options =>
//             {
//                 options.UseSqlite("DataSource=:memory:"); // In-memory database
//             });

//             // Build the service provider
//             var serviceProvider = services.BuildServiceProvider();

//             // Create a scope to obtain a reference to the database context (BlogDbContext)
//             using (var scope = serviceProvider.CreateScope())
//             {
//                 var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

//                 // Open connection to the in-memory database
//                 db.Database.OpenConnection();
                
//                 // Apply migrations
//                 db.Database.EnsureCreated();
//             }
//         });

//         builder.UseEnvironment("Development");
//     }
// }

using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing BlogDbContext registration
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Register the BlogDbContext to use the in-memory database
            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryBlogDb");
            });

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create a scope to apply migrations and ensure the database is created
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

                // Apply any pending migrations or ensure the schema is created
                db.Database.EnsureCreated();
            }
        });

        builder.UseEnvironment("Development"); // Use the Development environment
    }
}
