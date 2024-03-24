using PlatformService.Models;

namespace PlatformService.Data
{
    public static class Configure
    {
        public static void ConfigureDatabase(IApplicationBuilder app, bool IsProduction)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                SeedData(scope.ServiceProvider.GetService<ApplicationDbContext>(), IsProduction);
            }
        }

        private static void SeedData(ApplicationDbContext dbContext, bool IsProduction)
        {
            if (IsProduction)
            {
                try
                {
                    Console.WriteLine("--> Migrating database");
                    dbContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            var platformsAvailable = dbContext.Platforms.Any();
            if (!platformsAvailable)
            {
                System.Console.WriteLine("--> Seeding data");
                dbContext.Platforms.AddRange(
                    new Platform { Name = ".Net Core", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "Docker", Publisher = "Docker", Cost = "Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                dbContext.SaveChanges();
            }
        }
    }
}