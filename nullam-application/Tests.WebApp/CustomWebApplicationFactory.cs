using System.Globalization;
using App.DAL.EF;
using App.Domain;
using Base.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.WebApp;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    private static bool dbInitialized = false;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<AppDbContext>));
            
            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            // and new DbContext
            services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("InMemoryDbForTesting"); });
            
            // data seeding
            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
            
            db.Database.EnsureCreated();
            
            try
            {
                if (dbInitialized == false)
                {
                    dbInitialized = true;
                    // DataSeeder.SeedData(db);
                    
                    // Seed data
                    if (db.PaymentTypes.Any()) return;
            
                    var paymentTypeSularaha = new PaymentType()
                    {
                        Name = "Sularaha",
                        Comment = "Testing Sularaha"
                    };

                    var paymentTypeSwedbank = new PaymentType()
                    {
                        Name = "Swedbank",
                        Comment = "Testing Swedbank"
                    };

                    db.PaymentTypes.Add(paymentTypeSularaha);
                    db.PaymentTypes.Add(paymentTypeSwedbank);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }
        });
    }
}