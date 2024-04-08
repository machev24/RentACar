using System.Threading.Tasks;
using System.Collections.Generic;
using RentACar.Data.Models.Entities;

namespace RentACar.Data.Services
{
    public interface ICarsService
    {
        Task CreateAsync(CarServiceModel model);
        Task<IEnumerable<CarServiceModel>> GetAll();
        Task<CarServiceModel> GetCarByBrandAndModel(string brand, string model); 
    }
}
