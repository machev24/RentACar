using AutoMapper;
using RentACar.Data.Mapping;
using RentACar.Data.Services.Entities;

namespace RentACar.Data.Models
{
    public class RequestListingViewModel : IHaveCustomMapping
    {
        public CarListingViewModel Car { get; set; }

        public string UserName { get; set; }

        public DateTime OrderedOn { get; set; }

        public void ConfigureMapping(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<RequestServiceModel, RequestListingViewModel>()
                .ForMember(dest => dest.UserName, opt =>
                    opt.MapFrom(src => src.User.UserName));
        }
    }
}
