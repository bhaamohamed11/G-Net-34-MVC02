using GymManagementSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.ViewModels.TrainerViewModels
{
    public class TrainerToUpdateViewModel
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone Number Is Required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone must be a valid Egyptian number")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, 9000)]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(150, MinimumLength = 2)]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "City Is Required")]
        [StringLength(100, MinimumLength = 2)]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Specialties is required")]
        public Sepecialtes Specialties { get; set; }
    }
}
