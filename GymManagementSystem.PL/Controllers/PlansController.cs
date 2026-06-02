using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;
        public PlansController(IPlanService planService) => _planService = planService;

        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _planService.GetAllPlansAsync(ct));

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanDetailsAsync(id, ct);
            if (plan == null) return NotFound();
            return View(plan);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var model = await _planService.GetPlanToUpdateAsync(id, ct);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var success = await _planService.UpdatePlanAsync(id, model, ct);
            if (!success)
                TempData["ErrorMessage"] = "Cannot update a plan with active memberships";
            else
                TempData["SuccessMessage"] = "Plan updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var (success, message) = await _planService.TogglePlanStatusAsync(id, ct);
            if (success) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}