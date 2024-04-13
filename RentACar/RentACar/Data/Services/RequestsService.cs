using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RentACar.Data.Entities;
using RentACar.Data.Models.Entities;

namespace RentACar.Data.Services
{
    public class RequestsService : DataService, IRequestsService
    {
        private readonly IMapper _mapper;

        public RequestsService(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> Create(RequestServiceModel model, string userName)
        {
            if (!this.IsEntityStateValid(model))
            {
                return false;
            }

            var user = await this.context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            var car = await this.context.Cars.SingleOrDefaultAsync(c => c.Id == model.CarId);
            if (user == null || car == null)
            {
                return false;
            }

            var request = _mapper.Map<Request>(model);
            request.User = user;

            this.context.Cars.Update(car);
            await this.context.Requests.AddAsync(request);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<RequestServiceModel>> GetAll()
        {
            var requests = await this.context.Requests
                .ProjectTo<RequestServiceModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return requests;
        }

        public async Task<IEnumerable<RequestServiceModel>> GetAllForUser(string userName)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }

            var requests = await this.context.Requests
                .Where(r => r.UserId == user.Id)
                .ProjectTo<RequestServiceModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return requests;
        }

        public async Task<RequestServiceModel> AddRequestAsync(RequestServiceModel requestModel)
        {
            if (!this.IsEntityStateValid(requestModel))
            {
                return null;
            }

            // Map request model to entity
            var request = _mapper.Map<Request>(requestModel);

            // Add request to context and save changes
            await this.context.Requests.AddAsync(request);
            await this.context.SaveChangesAsync();

            // Map entity back to service model
            var createdRequest = _mapper.Map<RequestServiceModel>(request);

            return createdRequest;
        }
    }
}
