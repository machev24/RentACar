using RentACar.Data.Mapping;
using RentACar.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACar.Data.Models
{
    public class CarEditViewModel : IMapWith<CarServiceModel>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Brand { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Passenger Seats")]
        public int PassengerSeats { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price Per Day")]
        public decimal PricePerDay { get; set; }
    }
}
