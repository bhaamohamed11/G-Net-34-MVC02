using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepo;
        private readonly IGenericRepository<Session> _sessionRepo;

        public TrainerService(IGenericRepository<Trainer> trainerRepo, IGenericRepository<Session> sessionRepo)
        {
            _trainerRepo = trainerRepo;
            _sessionRepo = sessionRepo;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _trainerRepo.GetAllAsync(ct: ct);
            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Gender = t.Gender.ToString(),
                Specialties = t.Sepecialtes,
                Address = $"{t.Address.BuildingNumber} {t.Address.Street}, {t.Address.City}",
                DateOfBirth = t.DateofBirth.ToShortDateString()
            });
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int id, CancellationToken ct = default)
        {
            var t = await _trainerRepo.GetByIdAsync(id, ct);
            if (t == null) return null;
            return new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Gender = t.Gender.ToString(),
                Specialties = t.Sepecialtes,
                Address = $"{t.Address.BuildingNumber} {t.Address.Street}, {t.Address.City}",
                DateOfBirth = t.DateofBirth.ToShortDateString()
            };
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _trainerRepo.AnyAsync(t => t.Email == model.Email, ct);
            if (emailExists) return false;

            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateofBirth = model.DateOfBirth,
                Gender = model.Gender,
                Sepecialtes = model.Specialties,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber.ToString(),
                    Street = model.Street,
                    City = model.City
                }
            };
            return await _trainerRepo.AddAsync(trainer, ct) > 0;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
        {
            var t = await _trainerRepo.GetByIdAsync(id, ct);
            if (t == null) return null;
            return new TrainerToUpdateViewModel
            {
                Name = t.Name,
                Email = t.Email!,
                Phone = t.Phone,
                BuildingNumber = int.Parse(t.Address.BuildingNumber),
                Street = t.Address.Street,
                City = t.Address.City,
                Specialties = t.Sepecialtes
            };
        }

        public async Task<bool> UpdateTrainerAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(id, ct);
            if (trainer == null) return false;

            var emailTaken = await _trainerRepo.AnyAsync(t => t.Email == model.Email && t.Id != id, ct);
            if (emailTaken) return false;

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Sepecialtes = model.Specialties;
            trainer.Address.BuildingNumber = model.BuildingNumber.ToString();
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;
            trainer.UpdatedAt = DateTime.Now;
            return await _trainerRepo.UpdateAsync(trainer, ct) > 0;
        }

        public async Task<(bool success, string message)> DeleteTrainerAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _trainerRepo.GetByIdAsync(id, ct);
            if (trainer == null) return (false, "Trainer not found");

            var hasSessions = await _sessionRepo.AnyAsync(s => s.TrainerId == id, ct);
            if (hasSessions) return (false, "Cannot delete a trainer with sessions");

            await _trainerRepo.DeleteAsync(trainer, ct);
            return (true, "Trainer deleted successfully");
        }
    }
}
