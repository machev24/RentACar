using AutoMapper;
using RentACar.Data.Entities;
using RentACar.Data.Services.Entities;

namespace RentACar.Data.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarServiceModel>();
            CreateMap<Request, RequestServiceModel>();
        }
    }
}
