using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GymManagementSystem.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
namespace GymManagementSystem.DAL.Models
{
    public class GymUser : BaseEntity
    {

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required, MaxLength(100), EmailAddress]
        public string? Email { get; set; } = null!;
        [Required, MaxLength(11)]
        [RegularExpression(@"^(010||011||012||015)\d{8}$", ErrorMessage = "Egypt Phone Format Onlly.")]
        public string Phone { get; set; } = null!;
        public DateOnly DateofBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = null!;
    }

    [Owned]
    public class Address
    {

        public string BuildingNumber { get; set; }
        [Required, MaxLength(30)]
        public string City { get; set; } = null!;
        [Required, MaxLength(30)]
        public string Street { get; set; } = null!;
    }



}
