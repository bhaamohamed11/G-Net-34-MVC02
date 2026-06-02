using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Duration Is Required")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Is Required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(100, MinimumLength = 2)]
        public string Description { get; set; } = null!;
    }
}
