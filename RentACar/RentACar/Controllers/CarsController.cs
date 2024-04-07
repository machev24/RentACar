using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data.Models;
using RentACar.Data.Services;
using RentACar.Data.Services.Entities;

namespace RentACar.Web.Controllers
{
    public class CarsController : Controller
    {

        private readonly ICarsService _carsService;
        private readonly IRequestsService _requestsService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CarsController(ICarsService carsService, ILogger<CarsController> logger,
                       IRequestsService requestsService, IMapper mapper)
        {
            _carsService = carsService;
            _logger = logger;
            _requestsService = requestsService;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var allCars = await _carsService.GetAll();

            var allCarsArr = allCars as CarServiceModel[] ?? allCars.ToArray();

            var cars = allCarsArr
                .Select(_mapper.Map<CarListingViewModel>)
                .ToArray();

            return View(new AllCarsViewModel
            {
                Cars = cars
            });
        }

        [Authorize]
        public async Task<IActionResult> My()
        {
            var requests = (await _requestsService
                    .GetAllForUser(User.Identity.Name))
                .Select(_mapper.Map<RequestListingViewModel>);

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CarCreateBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var serviceModel = _mapper.Map<CarServiceModel>(model);

            await _carsService.CreateAsync(serviceModel);

            _logger.LogInformation("Event created: " + serviceModel.Brand, serviceModel);

            return RedirectToAction("All");
        }
    }
}

