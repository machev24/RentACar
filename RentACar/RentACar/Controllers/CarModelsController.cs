using Microsoft.AspNetCore.Mvc;

namespace RentACar.Controllers
{
    public class CarModelsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
