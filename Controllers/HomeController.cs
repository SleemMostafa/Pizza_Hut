using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pizza_Hut.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ICategoryRepository categoryRepository;

        public HomeController(ILogger<HomeController> logger, ICategoryRepository CatRepo)
        {
            _logger = logger;
            categoryRepository=CatRepo;
        }
        //public HomeController(ICategoryRepository cartRepo)
        //{
        //    categoryRepository=cartRepo;
        //}

        public IActionResult Index()
        {
            return View(categoryRepository.GetAll());
        }

        public IActionResult Privacy()
        {
            return View();
        }   
        [Authorize(Roles = "Admin")]

        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

    }
}
