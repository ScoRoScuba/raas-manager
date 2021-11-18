using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Services
{
    public interface IStaticRatesSchedulerService
    {
        bool Active();
        StaticRateDateTime GetActiveSchedule();
    }
}