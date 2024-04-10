using System.Threading.Tasks;
using System.Collections.Generic;
using RentACar.Data.Models.Entities;

namespace RentACar.Data.Services
{
    public interface ICarsService
    {
        Task<IEnumerable<CarServiceModel>> GetAll();
        Task<CarServiceModel> GetByIdAsync(string id);
        Task CreateAsync(CarServiceModel car);
        Task UpdateAsync(CarServiceModel car);
        Task<CarServiceModel> DeleteAsync(string id);
        Task<CarServiceModel> GetCarByBrandAndModel(string brand, string model);
    }
}
