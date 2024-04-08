using RentACar.Data.Mapping;
using RentACar.Data.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACar.Data.Models
{
    public class CarListingViewModel : IMapWith<CarServiceModel>
    {
        public string Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public int PassengerSeats { get; set; }

        public string Description { get; set; }

        public decimal PricePerDay { get; set; }
    }
}
