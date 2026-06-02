using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Data.DbContexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;

        public SessionService(IUnitOfWork unitofwork,IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _Mapper = mapper;
        }
        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var Sessions = await _unitOfWork._SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct:ct);
            if (Sessions?.Any() != true) return null;
            Sessions = Sessions.OrderByDescending(s => s.StartDate);
            var mappedSessions = _Mapper.Map<IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity-( await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct));
            }
            return mappedSessions;

        }
        public async Task<SessionViewModel?> GetSessionByIdAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepository.GetSessionWithTrainerAndCategoryAsync(SessionId, ct);
            if (session == null) return null;
            var mappedSession = _Mapper.Map<SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - (await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(mappedSession.Id, ct));
            return mappedSession;
        }
        #region Helper Methods
        private async Task<bool> IsSessionAvaliableAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepository.GetByIdAsync(SessionId, ct);
            if (session == null) return false;
            var Booked = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return Booked == 0;
        }
        #endregion
        public async Task<Result> CreateSessionAsynk(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
            return Result.Validation("End Date must be after Start Date");
            if(model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");
            var TrainerRepo= _unitOfWork.GetRepository<Trainer>();
            var Trainer= await TrainerRepo.GetByIdAsync(model.TrainerId, ct);
            if(Trainer is null)
                return Result.NotFound("Invalid Trainer Id");
            var CategoryRepo = _unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(model.CategoryId, ct);
            if (Category is null)
                return Result.NotFound("Category Not Found");
            var IsValidSepeciality = Enum.TryParse<Sepecialtes>(Category.CategoryName,true,out var CategorySepeciality);
            if(!IsValidSepeciality||Trainer.Sepecialtes!=CategorySepeciality)
                return Result.Validation("Trainer Sepeciality Does Not Match Session Category");
            var session = _Mapper.Map<Session>(model);
            await _unitOfWork._SessionRepository.AddAsync(session, ct);
            var AffectedRows= await _unitOfWork.SaveChangesAsync(ct);   
            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Create Session");
        }
        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default)
        {
            var session=_unitOfWork.GetRepository<Session>().GetByIdAsync(SessionId);
            if (session is null) return null;
            if (! await IsSessionAvaliableAsync(SessionId, ct)) return null;
            return _Mapper.Map<UpdateSessionViewModel>(session);



        }
      
        public async Task<Result> UpdateSessionAsync(int Id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var SessionRepo = _unitOfWork.GetRepository<Session>();
            var Session = await SessionRepo.GetByIdAsync(Id, ct);
            if (Session is null)
                return Result.NotFound("Session Not Found");
            if(Session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot Update Session That Has Already Started");
            var BookedCount=await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(Id, ct);
            if(BookedCount > 0)
                return Result.Fail("Cannot Update Session That Has Bookings");
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date must be after Start Date");
            if(model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);
            if (Trainer is null)
                return Result.NotFound("Invalid Trainer Id");
            var CategoryRepo = _unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(Session.CategoryId, ct);
            if (Category is null)
                return Result.NotFound("Category Not Found");
            var IsValidSepeciality = Enum.TryParse<Sepecialtes>(Category.CategoryName, true, out var CategorySepeciality);
            if (!IsValidSepeciality || Trainer.Sepecialtes != CategorySepeciality)
                return Result.Validation("Trainer Sepeciality Does Not Match Session Category");
            _Mapper.Map(model, Session);
            Session.UpdatedAt = DateTime.Now;
            await SessionRepo.UpdateAsync(Session);
            var AffectedRows = await _unitOfWork.SaveChangesAsync(ct);
            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");
        }
        public async Task<Result> RemoveSessionAsync(int SessionId, CancellationToken ct = default)
        {
            var SessionRepo = _unitOfWork.GetRepository<Session>();
            var Session = await SessionRepo.GetByIdAsync(SessionId, ct);
            if (Session is null)
                return Result.NotFound("Session Not Found");
            if (Session.EndDate>=DateTime.Now)
                return Result.Fail("Cannot Remove Session That Has Not Ended Yet");
            var BookedCount = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
            if (BookedCount > 0)
                return Result.Fail("Cannot Delete Session That Has Bookings");
            await SessionRepo.DeleteAsync(Session);
            var AffectedRows = await _unitOfWork.SaveChangesAsync(ct);
            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Remove Session");


        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsynk(CancellationToken ct = default)
        {
            var Trainers= await _unitOfWork.GetRepository<Category>().GetAllAsync(ct:ct);
            return _Mapper.Map<IEnumerable<CategorySelectViewModel>>(Trainers);
        }

      

      

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDownAsynk(CancellationToken ct = default)
        {
            var Trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _Mapper.Map<IEnumerable<TrainerSelectViewModel>>(Trainers);
        }


    }
}
