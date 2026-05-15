using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller

    {

        private readonly IPlanRepository planRepository;

        public PlansController(IPlanRepository planRepository)
        {
            this.planRepository = planRepository;
        }
        public async Task<IActionResult> Index()
        {
            var plans = await planRepository.GetAllAsync();
            return View(plans);
        }
        public async Task<IActionResult> Details(int id)
        {
            var plan = await planRepository.GetByIdAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);


        }
    }
}