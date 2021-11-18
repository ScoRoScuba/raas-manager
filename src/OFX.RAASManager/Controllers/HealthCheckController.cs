using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OFX.RAASManager.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : Controller
    {
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(ILogger<HealthCheckController> logger)
        {
            _logger = logger;
        }

        // GET: api/<controller>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            _logger.LogInformation("Health check received");

            return Ok("The Time is: " + DateTime.UtcNow.ToString("hh:mm:ss tt") + " in UTC");
        }
    }
}
