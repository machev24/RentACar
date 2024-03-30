using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RentACar.Data.Entities;
using RentACar.Data.Services.Entities;

namespace RentACar.Data.Services
{
    public class CarsService : DataService, ICarsService
    {
        private readonly IMapper _mapper;

        public CarsService(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task CreateAsync(CarServiceModel model)
        {
            if (!this.IsEntityStateValid(model))
            {
                return;
            }

            var carEntity = _mapper.Map<Car>(model);

            await this.context.AddAsync(carEntity);

            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CarServiceModel>> GetAll()
        {
            var cars = await this.context.Cars
                .Where(e => e.PricePerDay > 0)
                .ProjectTo<CarServiceModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CarServiceModel>>(cars);
        }
    }

}
