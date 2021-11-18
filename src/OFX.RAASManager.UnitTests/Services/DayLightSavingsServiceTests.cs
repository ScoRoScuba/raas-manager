using System;
using OFX.RAASManager.Services;
using Xunit;

namespace OFX.RAASManager.UnitTests.Services
{
    public class DayLightSavingsServiceTests
    {
        [Theory]

        [InlineData(2019, 04, 10, 11, 00, 00, true)]
        [InlineData(2019, 02, 10, 11, 00, 00, false)]
        [InlineData(2020, 03, 08, 03, 01, 00, true)]
        [InlineData(2019, 11, 03, 02, 00, 00, false)]
        [InlineData(2019, 03, 11, 02, 01, 00, true)]
        [InlineData(2019, 03, 10, 03, 00, 00, true)]
        public void IsNewYorkOnDayLightSavingTests(int year, int month, int day, int hour, int minute, int seconds, bool expectedResult)
        {
            var date = new DateTime(year, month, day, hour, minute, seconds);
            var dayLightSavingService = new DayLightSavingService();

            bool actualResult = dayLightSavingService.IsNewYorkOnDayLightSaving(date);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]

        [InlineData(2019, 04, 07, 01, 00, 00, true)]
        [InlineData(2019, 05, 06, 01, 00, 00, false)]
        [InlineData(2019, 03, 01, 00, 00, 00, true)]
        [InlineData(2019, 09, 29, 03, 01, 00, true)]
        [InlineData(2019, 12, 12, 03, 01, 00, true)]
        public void IsNewZealandOnDayLightSavingTests(int year, int month, int day, int hour, int minute, int seconds, bool expectedResult)
        {
            var date = new DateTime(year, month, day, hour, minute, seconds);
            var dayLightSavingService = new DayLightSavingService();

            bool actualResult = dayLightSavingService.IsNewZealandOnDayLightSaving(date);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
