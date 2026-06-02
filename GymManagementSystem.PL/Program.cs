using GymManagementSystem.BLL;
using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Data.DbContexts;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.PL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
#region service
builder.Services.AddDbContext<GymDbContext>(option=> {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IAnalyticsServices, AnalyticsServices>();
//builder.Services.AddScoped<IPlanRepository, PlanRepository>();
#endregion
var app = builder.Build();
await app.MigrateAndDataAsync();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
