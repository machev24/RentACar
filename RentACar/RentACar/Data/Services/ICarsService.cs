using RentACar.Data.Services.Entities;

namespace RentACar.Data.Services
{
    public interface ICarsService
    {
        Task CreateAsync(CarServiceModel model);

        Task<IEnumerable<CarServiceModel>> GetAll();
    }
}
