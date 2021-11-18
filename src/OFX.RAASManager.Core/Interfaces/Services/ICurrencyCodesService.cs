using System.Collections.Generic;

namespace OFX.RAASManager.Core.Interfaces.Services
{
    public interface ICurrencyCodesService
    {
        IEnumerable<string> GetStaticCurrencyCodes();
    }
}
