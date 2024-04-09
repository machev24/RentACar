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
            CreateMap<Car, CarServiceModel>();
            CreateMap<Car, CarListingViewModel>();
            CreateMap<CarServiceModel, Car>();
            CreateMap<CarServiceModel, CarEditViewModel>();
            CreateMap<CarEditViewModel, CarServiceModel>();
            CreateMap<CarServiceModel, CarListingViewModel>();
            CreateMap<CarServiceModel, CarDetailsViewModel>();
            CreateMap<CarCreateBindingModel, CarServiceModel>();
           
            CreateMap<Request, RequestServiceModel>();
            CreateMap<RequestServiceModel, Request>();
            CreateMap<RequestServiceModel, RequestListingViewModel >();
            CreateMap<RequestCreateBindingModel, RequestServiceModel>();

            CreateMap<User, UserListingViewModel>();
        }
    }
}
