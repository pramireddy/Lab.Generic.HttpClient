using Lab.WebApp.ApiGateway;
using Lab.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITestServiceGateway _testServiceGateway;

        public HomeController(ILogger<HomeController> logger, ITestServiceGateway testServiceGateway)
        {
            _logger = logger;
            _testServiceGateway = testServiceGateway;
        }

        public async Task<IActionResult> Index()
        {
            var result = await  _testServiceGateway.GetWeatherForecasts();

            return View();
        }

        public IActionResult Privacy()
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
