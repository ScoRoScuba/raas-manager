using System;
using TimeZoneConverter;

namespace OFX.RAASManager.Services
{
    public class DayLightSavingService : IDayLightSavingService
    {
        public bool IsNewYorkOnDayLightSaving(DateTime date)
        {
            var timeZoneInfo = TZConvert.GetTimeZoneInfo("Eastern Standard Time");
            return timeZoneInfo.IsDaylightSavingTime(date);
        }

        public bool IsNewZealandOnDayLightSaving(DateTime date)
        {
            var timeZoneInfo = TZConvert.GetTimeZoneInfo("New Zealand Standard Time");
            return timeZoneInfo.IsDaylightSavingTime(date);
        }
    }
}
