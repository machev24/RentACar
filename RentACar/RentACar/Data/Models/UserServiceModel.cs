using Microsoft.AspNetCore.Identity;
using RentACar.Data.Entities;
using RentACar.Data.Mapping;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Data.Models
{
    public class UserServiceModel : IdentityUser, IMapWith<User>
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string UniqueCitizenNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ICollection<Request> Requests { get; set; }
    }
}
