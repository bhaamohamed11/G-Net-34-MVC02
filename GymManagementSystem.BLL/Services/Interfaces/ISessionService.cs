using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<SessionViewModel?>GetSessionByIdAsync(int SessionId, CancellationToken ct = default);
        Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default);
        Task<Result>CreateSessionAsynk(CreateSessionViewModel model, CancellationToken ct = default);
        Task<Result>UpdateSessionAsync(int Id,UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result>RemoveSessionAsync(int SessionId, CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>>GetTrainerForDropDownAsynk(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsynk(CancellationToken ct = default);

    }
}
