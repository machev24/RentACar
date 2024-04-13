using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RentACar.Data.Models;
using RentACar.Data.Models.Entities;
using RentACar.Data.Services;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsService _carsService;
        private readonly IRequestsService _requestsService;
        private readonly IMapper _mapper;

        public CarsController(ICarsService carsService, IRequestsService requestsService, IMapper mapper)
        {
            _carsService = carsService;
            _requestsService = requestsService;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var allCars = await _carsService.GetAll();
            var carsViewModel = allCars.Select(_mapper.Map<CarListingViewModel>);

            var viewModel = new AllCarsViewModel
            {
                Cars = carsViewModel
            };

            return View(viewModel);
        }


        [Authorize]
        public async Task<IActionResult> My()
        {
            var requests = await _requestsService.GetAllForUser(User.Identity.Name);
            var requestViewModels = requests.Select(_mapper.Map<RequestListingViewModel>);
            return View(requestViewModels);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CarCreateBindingModel bindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(bindingModel);
            }

            var serviceModel = _mapper.Map<CarServiceModel>(bindingModel);
            await _carsService.CreateAsync(serviceModel);

            return RedirectToAction("All");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var car = await _carsService.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CarEditViewModel>(car);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, CarEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var existingCar = await _carsService.GetByIdAsync(id);
            if (existingCar == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, existingCar);
            await _carsService.UpdateAsync(existingCar);

            return RedirectToAction("All");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            var car = await _carsService.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CarDetailsViewModel>(car);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var car = await _carsService.GetByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CarDeleteViewModel>(car);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var deletedCar = await _carsService.DeleteAsync(id);
            if (deletedCar == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult AvailableCars()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AvailableCars(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                ModelState.AddModelError("", "End Date must be greater than Start Date.");
                return View(); // Return the view without passing any data
            }

            var availableCars = await _carsService.GetAvailableCars(startDate, endDate);
            var carsViewModel = availableCars.Select(_mapper.Map<CarListingViewModel>);
            return View(carsViewModel);
        }
    }
}