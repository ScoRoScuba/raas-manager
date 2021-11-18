using System.Collections.Generic;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Interfaces.Services
{
    public interface IStaticRatesService
    {
        IList<StaticRateDateTime> GetScheduledDateTimes();
        void SetScheduledDateTimes(IList<StaticRateDateTime> mongoStaticRateDateTime);
    }
}
