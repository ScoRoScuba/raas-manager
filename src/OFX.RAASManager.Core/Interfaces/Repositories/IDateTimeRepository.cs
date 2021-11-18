using System.Collections.Generic;
using System.Threading.Tasks;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Interfaces.Repositories
{
    public interface IDateTimeRepository
    {
        IList<StaticRateDateTime> GetDateTime();
        T Match<T>(ICriteria<T> criteria);
        Task SetDateTime(IList<StaticRateDateTime> mongoStaticRateDateTimes);
    }
}
