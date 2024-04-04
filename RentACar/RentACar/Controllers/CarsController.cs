using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Data.Entities;
using RentACar.Data.Models;
using RentACar.Data.Services;
using RentACar.Data.Services.Entities;

namespace RentACar.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsService _carsService;
        private readonly ILogger<CarsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRequestsService _requestsService;
        private readonly ApplicationDbContext _context;

        public CarsController(ICarsService carsService, ILogger<CarsController> logger, IMapper mapper, IRequestsService requestsService, ApplicationDbContext context)
        {
            _carsService = carsService;
            _logger = logger;
            _mapper = mapper;
            _requestsService = requestsService;
            _context = context;
        }

        public async Task<IActionResult> All()
        {
            var allCars = await _carsService.GetAll();

            var cars = allCars
                .OrderBy(car => car.Brand)
                .Select(_mapper.Map<CarListingViewModel>)
                .ToList();

            return View(new AllCarsViewModel
            {
                Cars = cars
            });
        }

        public async Task<IActionResult> My()
        {
            var userId = User.Identity.Name;

            var requests = await _requestsService.GetAllForUser(userId);

            var myRequests = requests.Select(request => new RequestListingViewModel
            {
                Car = _mapper.Map<CarListingViewModel>(request.Car),
                UserName = request.User.UserName,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            }).ToList();

            return View(myRequests);
        }

        // GET: Cars/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCreateBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var carServiceModel = _mapper.Map<CarServiceModel>(model);

            await _carsService.CreateAsync(carServiceModel);

            _logger.LogInformation($"Car created: {carServiceModel.Brand} {carServiceModel.Model}", carServiceModel);

            return RedirectToAction(nameof(All)); // Redirect to the All action to display the list of all cars
        }


        // GET: Cars/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CarEditBindingModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var car = _mapper.Map<Car>(model);

            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CarExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(All)); // Redirect to the All action to display the updated list
        }

        // GET: Cars/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(All)); // Redirect to the All action to display the updated list
        }

        private async Task<bool> CarExists(string id)
        {
            return await _context.Cars.AnyAsync(e => e.Id == id);
        }
    }
}
