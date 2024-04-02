using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data.Models;
using RentACar.Data.Services.Entities;
using RentACar.Data.Services;

namespace RentACar.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsService _carsService;
        private readonly ILogger<CarsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRequestsService _requestsService;

        public CarsController(ICarsService carsService, ILogger<CarsController> logger, IMapper mapper, IRequestsService requestsService)
        {
            _carsService = carsService;
            _logger = logger;
            _mapper = mapper;
            _requestsService = requestsService;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carsService.GetAll();

            var carViewModels = _mapper.Map<IEnumerable<CarListingViewModel>>(cars);

            return View(carViewModels);
        }

        public async Task<IActionResult> All(int? page)
        {
            if (!page.HasValue || page < 1)
            {
                page = 1;
            }

            var allCars = await _carsService.GetAll();

            var allCarsList = allCars.ToList();

            var cars = allCarsList
                .OrderBy(car => car.Brand)
                .Skip(10 * (page.Value - 1))
                .Take(10)
                .Select(_mapper.Map<CarListingViewModel>)
                .ToList();

            return View(new AllCarsViewModel
            {
                Cars = cars,
                CurrentPage = page.Value,
                PageCount = (int)Math.Ceiling((double)allCarsList.Count / 10)
            });
        }

        public async Task<IActionResult> My()
        {
            var userId = User.Identity.Name;

            var requests = await _requestsService.GetAllForUser(userId);

            var myCars = requests.Select(request => _mapper.Map<CarListingViewModel>(request.Car)).ToList();

            return View(myCars);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

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

            return RedirectToAction("All");
        }
    }
}
