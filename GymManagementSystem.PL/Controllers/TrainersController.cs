using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainersController(ITrainerService trainerService) => _trainerService = trainerService;

        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _trainerService.GetAllTrainersAsync(ct));

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var model = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (model == null) return NotFound();
            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var success = await _trainerService.CreateTrainerAsync(model, ct);
            if (!success) TempData["ErrorMessage"] = "Email already exists";
            else TempData["SuccessMessage"] = "Trainer added successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var model = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var success = await _trainerService.UpdateTrainerAsync(id, model, ct);
            if (!success) TempData["ErrorMessage"] = "Email already used by another trainer";
            else TempData["SuccessMessage"] = "Trainer updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var (success, message) = await _trainerService.DeleteTrainerAsync(id, ct);
            if (success) TempData["SuccessMessage"] = message;
            else TempData["ErrorMessage"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}