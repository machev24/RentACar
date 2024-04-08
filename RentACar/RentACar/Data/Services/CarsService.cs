using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RentACar.Data.Entities;
using RentACar.Data.Models.Entities;

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
            if (!IsEntityStateValid(model))
            {
                throw new ArgumentException("Invalid entity state.");
            }

            var carEntity = _mapper.Map<Car>(model);

            await context.Cars.AddAsync(carEntity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CarServiceModel>> GetAll()
        {
            var cars = await context.Cars
                .Where(e => e.PricePerDay > 0)
                .ProjectTo<CarServiceModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return cars;
        }
    }

}
