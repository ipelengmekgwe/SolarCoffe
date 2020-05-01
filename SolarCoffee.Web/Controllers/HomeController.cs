using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) => _logger = logger;


        [HttpGet("/api/")]
        public ActionResult WelcomeMessage()
        {
            _logger.LogInformation("Solar Coffee API has started!");
            return Ok("Solar Coffee API has started!");
        }
    }
}
