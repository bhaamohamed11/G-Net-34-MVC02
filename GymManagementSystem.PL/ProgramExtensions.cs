using GymManagementSystem.DAL.Data.DbContexts;
using GymManagementSystem.DAL.Data.DataSeeding;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
            { 
                logger.LogInformation($"Applying {PendingMigrations.Count()} Pending Migrations");
                await dbContext.Database.MigrateAsync();
            }
            var SeedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, SeedFolderPath, logger);
        }
    }
}