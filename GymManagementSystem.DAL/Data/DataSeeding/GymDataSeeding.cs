using Azure.Core.Serialization;
using GymManagementSystem.DAL.Data.DbContexts;
using GymManagementSystem.DAL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Data.DataSeeding
{
    public class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext context,string SeedFilePath, ILogger logger,CancellationToken ct=default)
        {
            try
            {
                var Plans=LoadDataFromJasonFile<Plan>("Plans.json",SeedFilePath);
                if(Plans.Count>0)
                {
                    context.Plans.AddRange(Plans);
                    logger.LogInformation("Seeded {Count} Plans",Plans.Count);
                }
                if(context.ChangeTracker.HasChanges())
                {
                    await context.SaveChangesAsync(ct);
                }
            }
            catch (Exception ex)
            { 
                   logger.LogError(ex,"Gym Data Seeding Failed");
                throw;
            }

        }
        
        private static List<T>LoadDataFromJasonFile<T>(string FileName,string FolderPath)
        {
         var FilePath=Path.Combine(FolderPath,FileName);
            if(!File.Exists(FilePath))
                throw new FileNotFoundException($"Seed Data File Not Found {FilePath}");
                var Data=File.ReadAllText(FilePath);
                var Options=new JsonSerializerOptions { PropertyNameCaseInsensitive =true};
                Options.Converters.Add(new JsonStringEnumConverter());
                return JsonSerializer.Deserialize<List<T>>(Data,Options) ??[];

            
        }
    }
}
