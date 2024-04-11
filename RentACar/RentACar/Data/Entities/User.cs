using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACar.Data.Entities
{
    [Table("AspNetUsers")]
    public class User : IdentityUser
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
