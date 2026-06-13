using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymManagementSystem.PL.Models;
<<<<<<< HEAD
using GymManagementSystem.BLL.Services.Interfaces;
=======

>>>>>>> ab71e25af933999c52642f7d155ce0ca029030c0
namespace GymManagementSystem.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
<<<<<<< HEAD
    private readonly IAnalyticsServices analyticsServices;

    public HomeController(ILogger<HomeController> logger,IAnalyticsServices analyticsServices)
    {
        _logger = logger;
        this.analyticsServices = analyticsServices;
    }

    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var Data = await analyticsServices.GetAnalyticsDataAsync(ct);
        return View(Data);
=======

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
>>>>>>> ab71e25af933999c52642f7d155ce0ca029030c0
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
