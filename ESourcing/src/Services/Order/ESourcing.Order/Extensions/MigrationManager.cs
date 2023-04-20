using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Ordering.Infrastructure.Data;

namespace ESourcing.Order.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            try
            {
                var orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
                if (orderContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    orderContext.Database.Migrate();
                }
                //OrderContextSeed.SeedAsync(orderContext).Wait();
            }
            catch (Exception ex)
            {

                throw;
            }
            return host;
        }

        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            try
            {
                var orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
                if (orderContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    orderContext.Database.EnsureCreated();
                    //OrderContextSeed.SeedAsync(orderContext).Wait();
                    
                    //orderContext.Database.Migrate();
                }
                //var pendingMigrations = orderContext.Database.GetAppliedMigrations().ToList();

                //var migrator = orderContext.Database.GetService<IMigrator>();
                //var migrations = orderContext.Database.GetAppliedMigrations();
                //var migrations2 = orderContext.Database.GetMigrations();
                //foreach (var migration in migrations2)
                //{
                //    migrator.Migrate(migration);
                //}
                //var migrations3 = orderContext.Database.GetPendingMigrations();

                //if (pendingMigrations.Any())
                //{
                //    //var migrator = orderContext.Database.GetService<IMigrator>();
                //    //var migrations = orderContext.Database.GetAppliedMigrations();
                //    //var migrations2 = orderContext.Database.GetMigrations();
                //    //var migrations3= orderContext.Database.GetPendingMigrations();

                //    foreach (var targetMigration in pendingMigrations)
                //        migrator.Migrate(targetMigration);
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
            return app;
        }


    }
}
