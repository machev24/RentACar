using RentACar.Data.Mapping;
using RentACar.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Data.Models
{
    public class RequestCreateBindingModel : IMapWith<RequestServiceModel>
    {
        [Required]
        public string CarId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
