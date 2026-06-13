using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var Sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync();
            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var ActiveMembers = await _unitOfWork.GetRepository<MemberShip>().CountAsync(m=>m.EndDate>DateTime.Now,ct);
            return new AnalyticsViewModel
            {
                TotalMembers = totalMembers,
                ActiveMembers = ActiveMembers,
                TotalTrainers = totalTrainers,
                UpcomingSessions = Sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = Sessions.Count(s => s.EndDate < DateTime.Now)
            };



        }
    }
}
