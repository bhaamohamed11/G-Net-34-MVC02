using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<Plan?> GetPlanDetailsAsync(int id, CancellationToken ct = default);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default);
        Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default);
        Task<(bool success, string message)> TogglePlanStatusAsync(int id, CancellationToken ct = default);
    }
}
