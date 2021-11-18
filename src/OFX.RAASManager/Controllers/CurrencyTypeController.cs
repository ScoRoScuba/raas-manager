using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OFX.RAASManager.Core.Interfaces.Services;

namespace OFX.RAASManager.Controllers
{
    [Route("currencyTypes")]
    [Produces("application/json")]
    public class CurrencyTypeController : ControllerBase
    {
        private readonly ICurrencyCodesService _currencyCodes;
        public CurrencyTypeController(ICurrencyCodesService currencyCodes)
        {
            _currencyCodes = currencyCodes;
        }

        /// <summary>
        /// This method is used to fetch all static currencies for StaticRates
        /// </summary>
        /// <returns>Currency List</returns>
        [HttpGet(Name="GetStaticCurrencyCodes")]
        [AllowAnonymous]
        public IActionResult Get()
        {
           return   Ok(_currencyCodes.GetStaticCurrencyCodes());          
        } 
    }
}
