using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.AttatchmentService;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;
        private readonly IAttatchmentService _attatchmentService;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper,IAttatchmentService attatchmentService)
        {
            _unitOfWork = unitOfWork;
            _Mapper = mapper;
            _attatchmentService = attatchmentService;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();

            var EmailExists = await MemberRepo.AnyAsync(m => m.Email == model.Email, ct);
            var PhoneExists = await MemberRepo.AnyAsync(m => m.Phone == model.Phone, ct);
            if (EmailExists || PhoneExists)
                return Result.Validation("Email Or Phone Already Exists");

            var Photo = await _attatchmentService.UploadAsync(
                model.Photo.OpenReadStream(), model.Photo.FileName, "MembersPictures", ct);

            if (string.IsNullOrEmpty(Photo))
                return Result.Validation("Profile Photo Upload Failed (Check File Type And Size)");

            var Member = _Mapper.Map<Member>(model);
            Member.Photo = Photo;
            await MemberRepo.AddAsync(Member, ct);
            var result=await _unitOfWork.SaveChangesAsync(ct);
            if (result == 0)
            {
                if (!string.IsNullOrEmpty(Member.Photo))
                {
                    _attatchmentService.Delete(Member.Photo, "Members");
                }
                return Result.Fail("Failed To Creat Member");
            }
            return Result.Ok();
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var Members = await MemberRepo.GetAllAsync(ct: ct);

            return _Mapper.Map<IEnumerable<MemberViewModel>>(Members);
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var Member = await MemberRepo.GetByIdAsync(memberId, ct);
            if (Member is null) return null;

            var MappedMember = _Mapper.Map<MemberViewModel>(Member);

            var MembershipRepo = _unitOfWork.GetRepository<MemberShip>();
            var ActiveMembership = await MembershipRepo.FirstOrDefaultAsync(
                m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct: ct);

            if (ActiveMembership != null)
            {
                var PlanRepo = _unitOfWork.GetRepository<Plan>();
                var Plan = await PlanRepo.GetByIdAsync(ActiveMembership.PlanId, ct);

                MappedMember.PlanName = Plan?.Name;
                MappedMember.MemberShipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                MappedMember.MemberShipEndDate = ActiveMembership.EndDate.ToShortDateString();
            }

            return MappedMember;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var HealthRecordRepo = _unitOfWork.GetRepository<HealthRecord>();
            var Record = await HealthRecordRepo.FirstOrDefaultAsync(hr => hr.MemberId == memberId, ct: ct);
            if (Record is null) return null;

            return _Mapper.Map<HealthRecordViewModel>(Record);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var Member = await MemberRepo.GetByIdAsync(memberId, ct);
            if (Member is null) return null;

            return _Mapper.Map<MemberToUpdateViewModel>(Member);
        }

        public async Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var Member = await MemberRepo.GetByIdAsync(memberId, ct);
            if (Member is null) return false;

            var BookingRepo = _unitOfWork.GetRepository<Booking>();
            var HasFutureSessions = await BookingRepo.AnyAsync(
                b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now, ct);
            if (HasFutureSessions) return false;

            var AffectedRows = await MemberRepo.DeleteAsync(Member, ct);
            return AffectedRows > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var Member = await MemberRepo.GetByIdAsync(memberId, ct);
            if (Member is null) return false;

            if (await MemberRepo.AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct))
                return false;

            if (await MemberRepo.AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct))
                return false;

            Member.Email = model.Email;
            Member.Phone = model.Phone;
            Member.Address.Street = model.Street;
            Member.Address.City = model.City;
            Member.Address.BuildingNumber = model.BuildingNumber.ToString();
            Member.UpdatedAt = DateTime.Now;

            var AffectedRows = await MemberRepo.UpdateAsync(Member, ct);
            return AffectedRows > 0;
        }
    }
}