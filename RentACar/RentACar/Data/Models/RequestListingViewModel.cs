using AutoMapper;
using RentACar.Data.Mapping;
using RentACar.Data.Models.Entities;

namespace RentACar.Data.Models
{
    public class RequestListingViewModel : IHaveCustomMapping
    {
        public CarListingViewModel Car { get; set; }

        public UserListingViewModel User { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public void ConfigureMapping(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<RequestServiceModel, RequestListingViewModel>()
                .ForMember(dest => dest.User, opt =>
                    opt.MapFrom(src => new UserListingViewModel { UserName = src.User.UserName }));
        }
    }
}