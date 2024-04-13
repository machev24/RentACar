using RentACar.Data.Entities;
using RentACar.Data.Models.Entities;

namespace RentACar.Data.Services
{
    public interface IRequestsService
    {
        Task<RequestServiceModel> AddRequestAsync(RequestServiceModel requestModel);
        Task<bool> Create(RequestServiceModel model, string userName);
        Task<IEnumerable<RequestServiceModel>> GetAll();
        Task<IEnumerable<RequestServiceModel>> GetAllForUser(string userName);
    }
}
