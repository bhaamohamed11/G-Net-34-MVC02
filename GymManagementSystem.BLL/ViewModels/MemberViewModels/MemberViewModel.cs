using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.ViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string?Photo { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        //Member Details
        public string DateOfBirth { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? PlanName { get; set; }= null!;
        public string? MemberShipStartDate { get; set; }= null!;
        public string? MemberShipEndDate { get; set; } = null!;


    }
}
