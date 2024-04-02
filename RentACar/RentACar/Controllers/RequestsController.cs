using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Data.Models;
using RentACar.Data.Services.Entities;
using RentACar.Data.Services;

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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var requests = (await _requestsService.GetAll())
                .Select(_mapper.Map<RequestListingViewModel>);

            return View(requests);
        }
    }
}
