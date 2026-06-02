using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Models
{
    public class Session:BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capsity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Booking> SessionMembers { get; set; } = null!;
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;


    }
}
