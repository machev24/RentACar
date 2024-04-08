using AutoMapper;
using RentACar.Data.Mapping;
using RentACar.Data.Models.Entities;

namespace RentACar.Data.Models
{
    public class RequestListingViewModel : IHaveCustomMapping
    {
        public CarListingViewModel Car { get; set; }

        public string UserName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public void ConfigureMapping(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<RequestServiceModel, RequestListingViewModel>()
                .ForMember(dest => dest.UserName, opt =>
                    opt.MapFrom(src => src.User.UserName));
        }
    }
}
