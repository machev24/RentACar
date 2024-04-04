using System.ComponentModel.DataAnnotations;

namespace RentACar.Data.Models
{
    public class CarEditBindingModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PassengerSeats { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PricePerDay { get; set; }
    }
}
