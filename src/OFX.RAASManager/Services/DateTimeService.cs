using System;
using OFX.RAASManager.Core.Interfaces;

namespace OFX.RAASManager.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UTCNow()
        {
            return DateTime.UtcNow;
        }
    }
}
