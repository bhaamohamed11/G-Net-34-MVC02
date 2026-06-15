using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Data.DbContexts
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();
                if (HasRoles && HasUsers) return;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    { 
                        new IdentityRole(){ Name="SuperAdmin"},
                        new IdentityRole(){ Name="Admin"} 
                    };

                    foreach (var roleName in Roles.Select(r=>r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                            if (!result.Succeeded)
                            {
                                logger.LogWarning("Failed to create role {Role}. Errors: {Errors}",
                                    roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                            }
                        }
                    }
                }

                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Rana",
                        LastName = "Hatem",
                        UserName = "RanaHatem",
                        Email = "RanaHatem@gmail.com",
                        PhoneNumber = "01200782778"
                    };

                    var mainResult = await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    if (mainResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                        logger.LogInformation("Seeded SuperAdmin {Email}", MainAdmin.Email);
                    }
                    else
                    {
                        logger.LogError("Failed to seed SuperAdmin: {Errors}",
                            string.Join(", ", mainResult.Errors.Select(e => e.Description)));
                    }

                    var Admin1 = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Ali",
                        UserName = "AhmedAli",
                        Email = "AhmedAli@gmail.com",
                        PhoneNumber = "01200782478"
                    };

                    var adminResult = await userManager.CreateAsync(Admin1, "P@ssw0rd");
                    if (!adminResult.Succeeded)
                    {
                        logger.LogError("Failed to seed Admin: {Errors}",
                           string.Join(", ", adminResult.Errors.Select(e => e.Description)));
                                return;
                    }
                    logger.LogInformation("Seeded Admin {Email}", Admin1.Email);
                    await userManager.AddToRoleAsync(Admin1, "Admin");
                    
                   
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding identity data.");
                throw;
            }
        }
    }
}