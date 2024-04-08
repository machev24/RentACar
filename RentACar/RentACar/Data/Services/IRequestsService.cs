using RentACar.Data.Models.Entities;

namespace RentACar.Data.Services
{
    public interface IRequestsService
    {
        Task<bool> Create(RequestServiceModel model, string userName);
        Task<IEnumerable<RequestServiceModel>> GetAll();
        Task<IEnumerable<RequestServiceModel>> GetAllForUser(string userName);
    }
}
