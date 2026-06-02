using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
     public class MemberShip:BaseEntity
    {
        public DateTime EndDate { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = default!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = default!;
        [NotMapped]
        public String Status => EndDate > DateTime.Now ? "Active" : "Expired";
        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;

    }
}
