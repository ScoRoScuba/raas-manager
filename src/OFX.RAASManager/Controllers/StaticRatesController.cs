using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OFX.RAASManager.Services;

namespace OFX.RAASManager.Controllers
{
    [Produces("application/json")]    
    public class StaticRatesController : Controller
    {
        private readonly IStaticRatesSchedulerService _staticRatesSchedulerService;

        public StaticRatesController(IStaticRatesSchedulerService staticRatesSchedulerService)
        {
            _staticRatesSchedulerService = staticRatesSchedulerService;
        }

        [HttpGet]
        [Route("staticrates/active")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new ActiveResponse(){IsActive = _staticRatesSchedulerService.Active() });
        }

        [HttpGet]
        [Route("staticrates/current/active")]
        [AllowAnonymous]
        public IActionResult GetActive()
        {
            if (_staticRatesSchedulerService.Active())
            {
                var data = _staticRatesSchedulerService.GetActiveSchedule();
                return Ok(data);
            }

            return NotFound();
        }
    }

    public class ActiveResponse
    {
        public bool IsActive { get; set; }
    }
}