using AutoMapper;
using RentACar.Data.Entities;
using RentACar.Data.Models;
using RentACar.Data.Models.Entities;

namespace RentACar.Data.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CarCreateBindingModel, CarServiceModel>();
            CreateMap<CarServiceModel, Car>();
            CreateMap<Car, CarServiceModel>();
            CreateMap<CarServiceModel, CarListingViewModel>();
            CreateMap<Request, RequestServiceModel>();
        }
    }
}
