using System.Collections.Generic;
using OFX.RAASManager.Core.Interfaces.Repositories;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Application.Services
{
    public class StaticRatesService : IStaticRatesService
    {
        private readonly IDateTimeRepository _dateTimeRepository;

        public StaticRatesService(IDateTimeRepository dateTimeRepository)
        {
            _dateTimeRepository = dateTimeRepository;
        }

        public IList<StaticRateDateTime> GetScheduledDateTimes()
        {
            return _dateTimeRepository.GetDateTime();
        }

        public void SetScheduledDateTimes(IList<StaticRateDateTime> mongoStaticRateDateTimes)
        {
            _dateTimeRepository.SetDateTime(mongoStaticRateDateTimes);
        }
    }
}
