using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarRentalApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Списък с марки и модели коли (за демонстрационни цели)
            var cars = new List<CarModel>
            {
                new CarModel { Brand = "Toyota", Model = "Corolla", Description = "Compact car" },
                new CarModel { Brand = "Honda", Model = "Civic", Description = "Economy car" },
                 new CarModel { Brand = "BMW", Model = "E46", Description = "Sport car" },
                  new CarModel { Brand = "Opel", Model = "Zafira", Description = "Family car" },
                   new CarModel { Brand = "Volkswagen", Model = "Golf", Description = "Economy car" },
                    new CarModel { Brand = "Mercedes", Model = "CLS", Description = "Sport car" },
                 new CarModel { Brand = "Porche", Model = "Panamera", Description = "Sport car" },
                  new CarModel { Brand = "Citroen", Model = "C4", Description = "Economy car" },

            };

            return View(cars);
        }
    }

    public class CarModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
    }
}

