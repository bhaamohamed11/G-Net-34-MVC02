using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels;
<<<<<<< HEAD
using GymManagementSystem.DAL.Data.DbContexts;
=======
using GymManagementSystem.DAL.DbContexts;
>>>>>>> ab71e25af933999c52642f7d155ce0ca029030c0
using GymManagementSystem;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _MemberRepristory;
        private readonly IGenericRepository<MemberShip> _MemberShipRepristory;
        private readonly IGenericRepository<Plan> _PlanRepristory;
        private readonly IGenericRepository<HealthRecord> _HealthRecordRepristory;
        private readonly IGenericRepository<Booking> _BookingRepristory;
        public MemberService(IGenericRepository<Member> MemberRepristory, IGenericRepository<MemberShip> MemberShipRepristory,IGenericRepository<Plan> PlanRepristory,IGenericRepository<HealthRecord>HealthRecordRepristory,IGenericRepository<Booking> BookingRepristory)
        {
            _MemberRepristory = MemberRepristory;
            _MemberShipRepristory = MemberShipRepristory;
            _PlanRepristory= PlanRepristory;
            _HealthRecordRepristory = HealthRecordRepristory;
            _BookingRepristory = BookingRepristory;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _MemberRepristory.AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await _MemberRepristory.AnyAsync(m => m.Phone == model.Phone, ct);
            if (emailExists || phoneExists) return false;
            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateofBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber.ToString(),
                    City = model.City,
                    Street = model.Street


                },
                HealthRecord = new HealthRecord
                {
                    Hight = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                }


            };
            var result = await _MemberRepristory.AddAsync(member);
            return result>0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _MemberRepristory.GetAllAsync(ct:ct);
            var memberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Gender=m.Gender.ToString(),

            });
            return memberViewModels;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _MemberRepristory.GetByIdAsync(memberId, ct);
            if (member == null) return null;
            var ViewModel = new MemberViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateofBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} {member.Address.Street} {member.Address.City}",
            };
            var ActiveMemberShip= await _MemberShipRepristory.FirstOrDefaultAsync(m=> m.MemberId == memberId && m.EndDate > DateTime.Now, ct:ct);
            if(ActiveMemberShip != null)
            {
                var plan = await _PlanRepristory.GetByIdAsync(ActiveMemberShip.PlanId, ct);
                ViewModel.PlanName = plan?.Name;
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }
            return ViewModel;
        }


        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await _HealthRecordRepristory.FirstOrDefaultAsync(hr => hr.MemberId == memberId, ct:ct);
            if (record == null) return null;
            return new HealthRecordViewModel
            {
                Height = record.Hight,
                Weight = record.Weight,
                BloodType = record.BloodType,
                Note = record.Note
            };

        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _MemberRepristory.GetByIdAsync(memberId, ct);
            if (member == null) return null;
           return new MemberToUpdateViewModel
            {
                Name = member.Name,
<<<<<<< HEAD
                Email = member .Email,
=======
                Email = member.Email,
>>>>>>> ab71e25af933999c52642f7d155ce0ca029030c0
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber = int.Parse(member.Address.BuildingNumber),
                Photo = member.Photo,
            };
        }

        public async Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
           var member= await _MemberRepristory.GetByIdAsync(memberId, ct);
            if (member == null) return false;
<<<<<<< HEAD
            var HasFutureSessions=await _BookingRepristory.AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now, ct);
=======
            var HasFutureSessions=await _BookingRepristory.AnyAsync(b => b.MemberId == memberId && b.Session.StartTime > DateTime.Now, ct);
>>>>>>> ab71e25af933999c52642f7d155ce0ca029030c0
            if (HasFutureSessions) return false;
            var result= await _MemberRepristory.DeleteAsync(member);
            return result > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _MemberRepristory.GetByIdAsync(memberId, ct);
            if (member == null) return false;
            if (await _MemberRepristory.AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct))
            return false;
            if (await _MemberRepristory.AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct))
                return false;

            member.Email= model.Email;
            member.Phone= model.Phone;
            member.Address.Street= model.Street;
            member.Address.City= model.City;
            member.Address.BuildingNumber= model.BuildingNumber.ToString();
            member.UpdatedAt= DateTime.Now;
            var result = await _MemberRepristory.UpdateAsync(member);
            return result > 0? true : false;


        }
    }
}
