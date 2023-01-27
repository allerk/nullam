using App.DAL.EF;
using Microsoft.EntityFrameworkCore;
using PaymentType = App.Domain.PaymentType;

namespace WebApp;

public static class AppDataHelper
{
    public static void SetupData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        using var serviceScope = app.
            ApplicationServices.
            GetRequiredService<IServiceScopeFactory>().
            CreateScope();

        using var context = serviceScope.
            ServiceProvider.
            GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services. No db context.");
        }
        
        if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory") return;

        if (configuration.GetValue<bool>("DataInitialization:DropDatabase"))
        {
            context.Database.EnsureDeleted();
        }
        
        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            Console.WriteLine("DataInitialization" + configuration.GetValue<bool>("DataInitialization:MigrateDatabase"));
            context.Database.Migrate();
        }
        if (configuration.GetValue<bool>("DataInitialization:SeedData"))
        {
            Console.WriteLine("Setting PaymentTypes");
            var listOfPaymentTypes = new List<PaymentType>()
            {
                new PaymentType()
                {
                  Name  = "Sularaha",
                  Comment = ""
                },
                new PaymentType()
                {
                  Name  = "Swedbank",
                  Comment = ""
                },
            };

            foreach (var paymentType in listOfPaymentTypes)
            {
                var elem = context.PaymentTypes.FirstOrDefault(x => x.Name == paymentType.Name);
                if (elem == null)
                {
                    context.PaymentTypes.Add(paymentType);   
                }
            }

            context.SaveChanges();
            context.Dispose();
        }
    }
}