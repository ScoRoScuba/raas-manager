using System.Collections.Generic;
using OFX.RAASManager.Core.Constants;
using OFX.RAASManager.Core.Interfaces.Services;

namespace OFX.RAASManager.Application.Services
{
    public class CurrencyCodesService : ICurrencyCodesService
    {
        public IEnumerable<string> GetStaticCurrencyCodes()
        {
            return new List<string>
            {
                StaticCurrencyCodes.AUD,
                StaticCurrencyCodes.CAD,
                StaticCurrencyCodes.EUR,
                StaticCurrencyCodes.GBP,
                StaticCurrencyCodes.HKD,
                StaticCurrencyCodes.JPY,
                StaticCurrencyCodes.NZD,
                StaticCurrencyCodes.SGD,
                StaticCurrencyCodes.USD,
                StaticCurrencyCodes.DKK,
                StaticCurrencyCodes.NOK,
                StaticCurrencyCodes.SEK,
                StaticCurrencyCodes.MXN,
                StaticCurrencyCodes.CHF,
                StaticCurrencyCodes.PLN,
            };

        }
    }
}
