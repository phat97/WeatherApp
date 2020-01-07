using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Weather.Models;
using Newtonsoft.Json;

namespace Weather.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private WeatherInformation weatherInformation;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var uri = "http://api.openweathermap.org/data/2.5/weather?q=Surrey,ca&appid=6a918a2e7455032b8e29775f764b46df";
            var client = new HttpClient();
            var data = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;


            weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(data);

            Console.WriteLine(weatherInformation.name);
            foreach( var item in weatherInformation.weather)
            {
                Console.WriteLine(item.main);
            }
            return View(weatherInformation);
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
