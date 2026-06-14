using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymManagementSystem.PL.Models;
using GymManagementSystem.BLL.Services.Interfaces;

namespace GymManagementSystem.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
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
