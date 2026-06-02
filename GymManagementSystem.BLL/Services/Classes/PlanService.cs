using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class PlanService:IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepo;
        private readonly IGenericRepository<MemberShip> _memberShipRepo;

        public PlanService(IGenericRepository<Plan> planRepo, IGenericRepository<MemberShip> memberShipRepo)
        {
            _planRepo = planRepo;
            _memberShipRepo = memberShipRepo;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepo.GetAllAsync(ct: ct);
            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationInDays,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }

        public async Task<Plan?> GetPlanDetailsAsync(int id, CancellationToken ct = default)
            => await _planRepo.GetByIdAsync(id, ct);

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepo.GetByIdAsync(id, ct);
            if (plan == null) return null;
            return new UpdatePlanViewModel
            {
                PlanName = plan.Name,
                DurationDays = plan.DurationInDays,
                Price = plan.Price,
                Description = plan.Description
            };
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _planRepo.GetByIdAsync(id, ct);
            if (plan == null) return false;

            var hasActiveMembers = await _memberShipRepo.AnyAsync(
                m => m.PlanId == id && m.EndDate > DateTime.Now, ct);
            if (hasActiveMembers) return false;

            plan.DurationInDays = model.DurationDays;
            plan.Price = model.Price;
            plan.Description = model.Description;
            plan.UpdatedAt = DateTime.Now;
            return await _planRepo.UpdateAsync(plan, ct) > 0;
        }

        public async Task<(bool success, string message)> TogglePlanStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await _planRepo.GetByIdAsync(id, ct);
            if (plan == null) return (false, "Plan not found");

            if (plan.IsActive)
            {
                var hasActiveMembers = await _memberShipRepo.AnyAsync(
                    m => m.PlanId == id && m.EndDate > DateTime.Now, ct);
                if (hasActiveMembers) return (false, "Cannot deactivate a plan with active memberships");
            }

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            await _planRepo.UpdateAsync(plan, ct);
            return (true, "Plan Status Changed");
        }
    }

}
