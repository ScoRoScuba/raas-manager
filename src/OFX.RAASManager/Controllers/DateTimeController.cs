using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Entities;
using OFX.RAASManager.ViewModel;

namespace OFX.RAASManager.Controllers
{
    [Produces("application/json")]
    [Route("scheduledDateTimes")] 
    public class DateTimeController : ControllerBase
    {
        private readonly IStaticRatesService _staticRatesService;
        private readonly IMapper _mapper;

        public DateTimeController(IStaticRatesService staticRatesService, IMapper mapper)
        {
            _staticRatesService = staticRatesService;
            _mapper = mapper;
        }

        /// <summary>
        /// This method is used to fetch all Static Rates Date Time
        /// </summary>
        /// <returns>Static Rates Date Time list</returns>
        [HttpGet(Name = "GetDateTime")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var dateList = _staticRatesService.GetScheduledDateTimes();

            return Ok(dateList);
        }

        [HttpPost(Name = "SetDateTime")]
        [AllowAnonymous]
        public IActionResult Post([FromBody] IList<DateTimeViewModel> dateTimeViewModels)
        {
            var dateTimeModel = ConvertViewModelsToModel(dateTimeViewModels);

            _staticRatesService.SetScheduledDateTimes(dateTimeModel);

            return Ok();
        }

        private IList<StaticRateDateTime> ConvertViewModelsToModel(IEnumerable<DateTimeViewModel> dateTimeViewModels)
        {
            return _mapper.Map<IList<StaticRateDateTime>>(dateTimeViewModels);
        }
    }
}