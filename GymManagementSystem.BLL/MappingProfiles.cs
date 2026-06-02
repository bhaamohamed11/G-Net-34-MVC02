using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        { 
            MapSession();
        }
    #region Session Mapping Profiles
    private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel,Session>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category,CategorySelectViewModel>();








        }

        #endregion
    } 
}