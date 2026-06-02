using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace GymManagementSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Index
        public async Task<IActionResult> Index(CancellationToken ct = default) =>
            View(await _sessionService.GetAllSessionsAsync(ct));
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownAsync(ct);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model,CancellationToken ct)
        {

            if (!ModelState.IsValid)
            {
                await PopulateDropdownAsync(ct);
                return View(model);
            }
        var Result= await _sessionService.CreateSessionAsynk(model, ct);
            if (Result.Success)
            {
                TempData["SuccessMessage"] = "Session created successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = Result.Error;
            await PopulateDropdownAsync(ct);
            return View(model);

        }

        #endregion
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if(session is null)
            {
               TempData["ErrorMessage"] = "Session not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(session);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if (session is null)
            { 
                TempData["ErrorMessage"] = "Session not found!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdownAsync(ct);
            return View(session);   
        
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            { 
              await PopulateDropdownAsync(ct);
                return View(model);
            }
            var Result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (Result.Success)
            {
                TempData["SuccessMessage"] = "Session updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = Result.Error;
            await PopulateDropdownAsync(ct);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await _sessionService.RemoveSessionAsync(id, ct);
            if (Result.Success)
            {
                TempData["SuccessMessage"] = "Session deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = Result.Error;
            return RedirectToAction(nameof(Index));
        }
        private async Task PopulateDropdownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainerForDropDownAsynk(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoryForDropDownAsynk(ct), "Id", "CategoryName");
        }




    }
}
