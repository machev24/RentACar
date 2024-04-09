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

        public CarsService(ApplicationDbContext context, IMapper mapper): base(context)
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

        public async Task<CarServiceModel> GetByIdAsync(string id)
        {
            var carEntity = await context.Cars.FindAsync(id);

            return _mapper.Map<CarServiceModel>(carEntity);
        }

        public async Task<CarServiceModel> GetCarByBrandAndModel(string brand, string model)
        {
            var carEntity = await context.Cars
                .FirstOrDefaultAsync(c => c.Brand == brand && c.Model == model);

            return _mapper.Map<CarServiceModel>(carEntity);
        }

        public async Task UpdateAsync(CarServiceModel model)
        {
            if (!IsEntityStateValid(model))
            {
                throw new ArgumentException("Invalid entity state.");
            }

            var existingCarEntity = await context.Cars.FindAsync(model.Id);
            if (existingCarEntity == null)
            {
                throw new InvalidOperationException("Car not found.");
            }

            _mapper.Map(model, existingCarEntity);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var carEntity = await context.Cars.FindAsync(id);
            if (carEntity == null)
            {
                throw new InvalidOperationException("Car not found.");
            }

            context.Cars.Remove(carEntity);
            await context.SaveChangesAsync();
        }
    }
}
