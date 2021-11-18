using System;
using System.Collections.Generic;
using System.Linq;
using OFX.RAASManager.Core.Interfaces;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Entities;
using OFX.RAASManager.Extensions;

namespace OFX.RAASManager.Services
{
    public class StaticRatesSchedulerService : IStaticRatesSchedulerService
    {
        private readonly IStaticRatesService _staticRatesService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IDayLightSavingService _dayLightSavingService;
        private readonly ILoggerService _loggerService;

        public StaticRatesSchedulerService(IStaticRatesService staticRatesService, IDateTimeService dateTimeService,
            IDayLightSavingService dayLightSavingService, ILoggerService loggerService)
        {
            _staticRatesService = staticRatesService;
            _dateTimeService = dateTimeService;
            _dayLightSavingService = dayLightSavingService;
            _loggerService = loggerService;
        }

        public bool Active()
        {
            if (GetActiveSchedule() != null) return true;

            return false;
        }
        
        public StaticRateDateTime GetActiveSchedule()
        {
            var scheduledDateTimes = GetScheduledStaticRateDates();

            var timeNow = _dateTimeService.UTCNow();

            var newYorkDayLightCorrection = _dayLightSavingService.IsNewYorkOnDayLightSaving(timeNow) ? 0 : 60;

            var newZealandDayLightCorrection = _dayLightSavingService.IsNewZealandOnDayLightSaving(timeNow) ? -60 : 0;

            var weekendOnly = scheduledDateTimes.FirstOrDefault(sdt => sdt.StopDateUTC == null && sdt.StartDateUTC == null);

            if (weekendOnly != null)
            {
                DateTime friday;
                DateTime sunday;

                if (timeNow.DayOfWeek == DayOfWeek.Saturday || timeNow.DayOfWeek == DayOfWeek.Sunday)
                {
                    friday = timeNow.AddDays(-2).ClosestWeekDay(DayOfWeek.Friday).Date.AddMinutes(weekendOnly.StartTimeUTCInMinutes +
                                                                                                  newYorkDayLightCorrection);
                }
                else
                {
                    friday = timeNow.ClosestWeekDay(DayOfWeek.Friday).Date.AddMinutes(weekendOnly.StartTimeUTCInMinutes +
                                                                                      newYorkDayLightCorrection);
                }

                sunday = timeNow.ClosestWeekDay(DayOfWeek.Sunday).Date.AddMinutes(weekendOnly.StopTimeUTCInMinutes +
                                                                                  newZealandDayLightCorrection);

                if (timeNow >= friday && timeNow <= sunday)
                {
                    return new StaticRateDateTime()
                    { 
                        StartDateUTC = friday.Date,
                        StopDateUTC = sunday.Date,
                        StartTimeUTCInMinutes = weekendOnly.StartTimeUTCInMinutes + newYorkDayLightCorrection,
                        StopTimeUTCInMinutes = weekendOnly.StopTimeUTCInMinutes + newZealandDayLightCorrection
                    };
                }
            }

            var scheduledDates = scheduledDateTimes.Where(sdt => sdt.StopDateUTC != null && sdt.StartDateUTC != null);
            foreach (var date in scheduledDates)
            {
                var startDate = date.StartDateUTC.Value.Date.AddMinutes(date.StartTimeUTCInMinutes + newYorkDayLightCorrection);
                var endDate = date.StopDateUTC.Value.Date.AddMinutes(date.StopTimeUTCInMinutes + newZealandDayLightCorrection);

                if (timeNow >= startDate && timeNow <= endDate)
                {
                    return date;
                }
            }

            _loggerService.Warn("StaticRatesSchedulerService:GetActiveSchedule:NoStaticRateDefined");

            return null;
        }

        private IList<StaticRateDateTime> GetScheduledStaticRateDates()
        {
            var scheduledDateTimes = _staticRatesService.GetScheduledDateTimes();
            if (!scheduledDateTimes.Any())
            {
                return new List<StaticRateDateTime>
                {
                    new StaticRateDateTime()
                    {
                        StartTimeUTCInMinutes = 1260,
                        StopTimeUTCInMinutes = 1260
                    }
                };
            }

            return scheduledDateTimes;
        }
    }
}
