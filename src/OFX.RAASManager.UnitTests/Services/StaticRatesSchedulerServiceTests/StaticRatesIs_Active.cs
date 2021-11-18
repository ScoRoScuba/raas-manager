using System;
using System.Collections.Generic;
using Moq;
using OFX.RAASManager.Core.Interfaces;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Entities;
using OFX.RAASManager.Services;
using Xunit;

namespace OFX.RAASManager.UnitTests.Services.StaticRatesSchedulerServiceTests
{
    public class StaticRatesIs_Active
    {
        [Fact]
        public void WhenNothingScheduledReturnsFalseWhenAskedIfActive()
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>());

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object, mockDateTimeService.Object,
                mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            bool result = staticRateSchedulerService.Active();

            Assert.False(result);
        }

        [Theory]

        [InlineData(2019, 06, 7, 20, 00, 00)]
        [InlineData(2019, 06, 6, 09, 00, 00)]
        [InlineData(2019, 06, 9, 21, 01, 00)]
        [InlineData(2019, 06, 10, 08, 01, 00)]
        [InlineData(2019, 06, 11, 20, 00, 00)]
        public void WhenWeekendOnlyIsScheduledReturnsFalseForWeekDay(int year, int month, int day, int hour, int minute, int seconds)
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockDayLightSavingService.Setup(ds => ds.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);
            mockDayLightSavingService.Setup(ds => ds.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(false);

            mockDateTimeService.Setup(ts => ts.UTCNow()).Returns(new DateTime(year, month, day, hour, minute, seconds));

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>
            {
                new StaticRateDateTime
                {
                    StartDateUTC = null,
                    StopDateUTC = null,
                    StartTimeUTCInMinutes = 1260,
                    StopTimeUTCInMinutes = 1260
                }
            });

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object, mockDateTimeService.Object,
                mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            bool result = staticRateSchedulerService.Active();

            Assert.False(result);
        }

        [Theory]

        [InlineData(2019, 06, 7, 21, 00, 00)]
        [InlineData(2019, 06, 7, 21, 01, 00)]
        [InlineData(2019, 06, 7, 22, 00, 00)]
        [InlineData(2019, 06, 8, 20, 00, 00)]
        [InlineData(2019, 06, 9, 20, 00, 00)]
        public void WhenWeekendOnlyIsScheduledReturnsTrueForWeekend(int year, int month, int day, int hour, int minute, int seconds)
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockDayLightSavingService.Setup(ds => ds.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);
            mockDayLightSavingService.Setup(ds => ds.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(false);

            mockDateTimeService.Setup(ts => ts.UTCNow()).Returns(new DateTime(year, month, day, hour, minute, seconds));

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>
            {
                new StaticRateDateTime
                {
                    StartDateUTC = null,
                    StopDateUTC = null,
                    StartTimeUTCInMinutes = 1260,
                    StopTimeUTCInMinutes = 1260
                }
            });

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object, mockDateTimeService.Object,
                mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            bool result = staticRateSchedulerService.Active();

            Assert.True(result);
        }

        [Fact]
        public void WhenScheduledAndTodayIsNotInScheduleReturnsFalse()
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockDayLightSavingService.Setup(ds => ds.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);
            mockDayLightSavingService.Setup(ds => ds.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(false);

            mockDateTimeService.Setup(ts => ts.UTCNow()).Returns(new DateTime(2019, 6, 10, 9, 0, 0));

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>
            {
                new StaticRateDateTime
                {
                    StartDateUTC = new DateTime(2019, 6, 11, 9, 0, 0),
                    StopDateUTC = new DateTime(2019, 6, 14, 9, 0, 0),
                    StartTimeUTCInMinutes = 1260,
                    StopTimeUTCInMinutes = 1260
                }
            });

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object, mockDateTimeService.Object,
                mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            bool result = staticRateSchedulerService.Active();

            Assert.False(result);
        }

        [Fact]
        public void WhenScheduledAndTodayIsBetweenScheduleReturnsTrue()
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockDayLightSavingService.Setup(ds => ds.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);
            mockDayLightSavingService.Setup(ds => ds.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(false);

            mockDateTimeService.Setup(ts => ts.UTCNow()).Returns(new DateTime(2019, 6, 12, 9, 0, 0));

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>
            {
                new StaticRateDateTime
                {
                    StartDateUTC = new DateTime(2019, 6, 11, 9, 0, 0),
                    StopDateUTC = new DateTime(2019, 6, 14, 9, 0, 0),
                    StartTimeUTCInMinutes = 1260,
                    StopTimeUTCInMinutes = 1260
                }
            });

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object, mockDateTimeService.Object,
                mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            bool result = staticRateSchedulerService.Active();

            Assert.True(result);
        }

        [Theory]

        [InlineData(2019, 7, 4, 21, 0, 0, true, false, false)]
        [InlineData(2019, 7, 5, 21, 0, 0, true, false, true)]
        [InlineData(2019, 7, 6, 21, 0, 0, true, false, true)]
        [InlineData(2019, 7, 7, 21, 1, 0, true, false, false)]
        [InlineData(2019, 7, 17, 21, 0, 0, true, false, false)]
        [InlineData(2019, 7, 18, 21, 0, 0, true, false, true)]
        [InlineData(2019, 7, 19, 21, 0, 0, true, false, true)]
        [InlineData(2019, 7, 20, 21, 0, 0, true, false, true)]
        [InlineData(2019, 7, 21, 21, 0, 0, true, false, true)]
        [InlineData(2019, 7, 22, 21, 0, 0, true, false, true)]
        [InlineData(2019, 7, 23, 21, 0, 0, true, false, false)]
        [InlineData(2019, 7, 22, 22, 0, 0, true, true, false)]
        [InlineData(2019, 7, 18, 19, 0, 0, false, true, false)]
        [InlineData(2019, 7, 18, 20, 0, 0, true, true, false)]
        [InlineData(2019, 7, 18, 20, 0, 0, false, true, false)]
        [InlineData(2019, 7, 12, 21, 0, 0, true, true, true)]
        [InlineData(2019, 7, 12, 20, 0, 0, true, true, false)]
        public void GivenScheduleReactsAccordingly(int todayYear, int todayMonth, int todayDay, int todayHour, int todayMinute, int todaySecond,
            bool nyDayLightSaving, bool nzDayLightSaving, bool expectedTestResult)
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockDayLightSavingService.Setup(ds => ds.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(nyDayLightSaving);
            mockDayLightSavingService.Setup(ds => ds.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(nzDayLightSaving);

            mockDateTimeService.Setup(ts => ts.UTCNow()).Returns(new DateTime(todayYear, todayMonth, todayDay, todayHour, todayMinute, todaySecond));

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>
            {
                new StaticRateDateTime
                {
                    StartTimeUTCInMinutes = 1260,
                    StopTimeUTCInMinutes = 1260
                },
                new StaticRateDateTime
                {
                    StartDateUTC = new DateTime(2019, 7, 18),
                    StopDateUTC = new DateTime(2019, 7, 22),
                    StartTimeUTCInMinutes = 1260,
                    StopTimeUTCInMinutes = 1260
                }
            });

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object, mockDateTimeService.Object,
                mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            bool actualResult = staticRateSchedulerService.Active();

            Assert.Equal(expectedTestResult, actualResult);
        }
    }
}

