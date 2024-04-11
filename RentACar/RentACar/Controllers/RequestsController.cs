using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data.Models;
using RentACar.Data.Services;
using RentACar.Data.Mapping;
using RentACar.Data.Models.Entities;

namespace RentACar.Web.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IRequestsService _requestsService;
        private readonly IMapper _mapper;

        public RequestsController(IRequestsService requestsService, IMapper mapper)
        {
            _requestsService = requestsService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestCreateBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("All", "Cars");
            }

            var serviceModel = _mapper.Map<RequestServiceModel>(model);

            var result = await _requestsService.Create(serviceModel, User.Identity.Name);
            if (!result)
            {
                return RedirectToAction("All", "Cars");
            }

            return RedirectToAction("My", "Cars");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var requests = await _requestsService.GetAll();

            if (User.IsInRole("Admin"))
            {
                var adminViewModels = requests.Select(_mapper.Map<RequestListingViewModel>);
                return View("AdminRequestView", adminViewModels);
            }
            else
            {
                var userViewModels = requests.Select(_mapper.Map<CarListingViewModel>);
                return View("UserRequestView", userViewModels);
            }
        }
    }
}
