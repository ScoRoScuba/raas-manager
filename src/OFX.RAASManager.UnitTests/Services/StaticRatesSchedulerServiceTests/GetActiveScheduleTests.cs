using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using OFX.RAASManager.Core.Interfaces;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Entities;
using OFX.RAASManager.Services;
using Xunit;

namespace OFX.RAASManager.UnitTests.Services.StaticRatesSchedulerServiceTests
{
    public class GetActiveScheduleTests
    {

        [Fact]
        public void WhenStandardWeekendReturnsWeekendOnlySchedule_WithAllDaylightSavingsOn()
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>());

            mockDateTimeService.Setup(dts => dts.UTCNow()).Returns(new DateTime(2019, 11, 9, 03, 00, 00));

            mockDayLightSavingService.Setup(dls => dls.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);
            mockDayLightSavingService.Setup(dls => dls.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object,
                mockDateTimeService.Object, mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            var expectedSchedule = new StaticRateDateTime
            {
                StartDateUTC =new DateTime(2019, 11, 8, 0, 0, 0, 0),
                StopDateUTC = new DateTime(2019, 11, 10, 0, 0, 0, 0),
                StartTimeUTCInMinutes = 1260,
                StopTimeUTCInMinutes = 1200
            };

            var result = staticRateSchedulerService.GetActiveSchedule();

            Assert.Equal(expectedSchedule.StartDateUTC.Value, result.StartDateUTC.Value);
            Assert.Equal(expectedSchedule.StopDateUTC.Value, result.StopDateUTC.Value);
            Assert.Equal(expectedSchedule.StartTimeUTCInMinutes, result.StartTimeUTCInMinutes);
            Assert.Equal(expectedSchedule.StopTimeUTCInMinutes, result.StopTimeUTCInMinutes);
        }

        [Fact]
        public void WhenWeekendRateInDBReturnsWeekendOnlySchedule_WithAllDaylightSavingsOn()
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>
            {
                new StaticRateDateTime
                {
                    StartTimeUTCInMinutes = 1260,
                    StopTimeUTCInMinutes = 1260
                }
            });

            mockDateTimeService.Setup(dts => dts.UTCNow()).Returns(new DateTime(2019, 11, 9, 03, 00, 00));

            mockDayLightSavingService.Setup(dls => dls.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);
            mockDayLightSavingService.Setup(dls => dls.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object,
                mockDateTimeService.Object, mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            var expectedSchedule = new StaticRateDateTime
            {
                StartDateUTC = new DateTime(2019, 11, 8, 0, 0, 0, 0),
                StopDateUTC = new DateTime(2019, 11, 10, 0, 0, 0, 0),
                StartTimeUTCInMinutes = 1260,
                StopTimeUTCInMinutes = 1200
            };

            var result = staticRateSchedulerService.GetActiveSchedule();

            Assert.Equal(expectedSchedule.StartDateUTC.Value, result.StartDateUTC.Value);
            Assert.Equal(expectedSchedule.StopDateUTC.Value, result.StopDateUTC.Value);
            Assert.Equal(expectedSchedule.StartTimeUTCInMinutes, result.StartTimeUTCInMinutes);
            Assert.Equal(expectedSchedule.StopTimeUTCInMinutes, result.StopTimeUTCInMinutes);
        }

        [Fact]
        public void WhenWeekendRateInDBOffBy2ReturnsWeekendOnlySchedule_WithAllDaylightSavingsOn()
        {
            var mockStaticRateService = new Mock<IStaticRatesService>();
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockDayLightSavingService = new Mock<IDayLightSavingService>();

            mockStaticRateService.Setup(srs => srs.GetScheduledDateTimes()).Returns(new List<StaticRateDateTime>
            {
                new StaticRateDateTime
                {
                    StartTimeUTCInMinutes = 1262,
                    StopTimeUTCInMinutes = 1260
                }
            });

            mockDateTimeService.Setup(dts => dts.UTCNow()).Returns(new DateTime(2019, 11, 9, 03, 00, 00));

            mockDayLightSavingService.Setup(dls => dls.IsNewYorkOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);
            mockDayLightSavingService.Setup(dls => dls.IsNewZealandOnDayLightSaving(It.IsAny<DateTime>())).Returns(true);

            var staticRateSchedulerService = new StaticRatesSchedulerService(mockStaticRateService.Object,
                mockDateTimeService.Object, mockDayLightSavingService.Object, new Mock<ILoggerService>().Object);

            var expectedSchedule = new StaticRateDateTime
            {
                StartDateUTC = new DateTime(2019, 11, 8, 0, 0, 0, 0),
                StopDateUTC = new DateTime(2019, 11, 10, 0, 0, 0, 0),
                StartTimeUTCInMinutes = 1262,
                StopTimeUTCInMinutes = 1200
            };

            var result = staticRateSchedulerService.GetActiveSchedule();

            Assert.Equal(expectedSchedule.StartDateUTC.Value, result.StartDateUTC.Value);
            Assert.Equal(expectedSchedule.StopDateUTC.Value, result.StopDateUTC.Value);
            Assert.Equal(expectedSchedule.StartTimeUTCInMinutes, result.StartTimeUTCInMinutes);
            Assert.Equal(expectedSchedule.StopTimeUTCInMinutes, result.StopTimeUTCInMinutes);
        }
    }
}
