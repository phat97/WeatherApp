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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Weather.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IActionContextAccessor _accessor;

        private readonly string indexPath = "~/Views/Home/index.cshtml";
        private WeatherInformation weatherInformation;
        private string openWeatherUri = "http://api.openweathermap.org/data/2.5/weather?";

        public HomeController(ILogger<HomeController> logger, IActionContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            string uri;
            var ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            var ipDetails = GetUserLocationDetailsyByIp(ip);

            if(ipDetails.Longitude == null)
            {
                uri = "http://api.openweathermap.org/data/2.5/weather?q=Surrey,ca&appid=6a918a2e7455032b8e29775f764b46df";
            } else
            {
                uri = openWeatherUri + "lat=" + ipDetails.Latitude + "&lon=" + ipDetails.Longitude + "&appid=6a918a2e7455032b8e29775f764b46df";
            }

            var client = new HttpClient();
            var data = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;

            weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(data);
            return View(indexPath, weatherInformation);
        }

        [HttpPost]
        public IActionResult CitySearch(string city, string country)
        {

            string uri = openWeatherUri +"q=" + city + "," + country +"&appid=6a918a2e7455032b8e29775f764b46df";

            var client = new HttpClient();
            var data = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;

            weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(data);
            return View(indexPath, weatherInformation);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private IpInfo GetUserLocationDetailsyByIp(string ip)
        {
            var ipInfo = new IpInfo();
            try
            {
                var uri = "http://api.ipstack.com/" + ip + "?access_key=c070cb44b8f7a3d72db4890feff1238d";
                var client = new HttpClient();
                var data = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(data);           
            }
            catch (Exception ex)
            {
                return null;
            }

            return ipInfo;
        }
    }
}
