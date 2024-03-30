using AutoMapper;

namespace RentACar.Data.Mapping
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(IMapperConfigurationExpression mapper);
    }
}
