using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OFX.RAASManager.Controllers;
using OFX.RAASManager.Core.Interfaces.Services;
using Xunit;

namespace OFX.RAASManager.UnitTests.Services
{
    public class CurrencyTypeTests
    {
        private CurrencyTypeController _controller;
        Mock<ICurrencyCodesService> _mockCurrencyCodes =new Mock<ICurrencyCodesService>();

        public CurrencyTypeTests()
        {
            _controller = new CurrencyTypeController(_mockCurrencyCodes.Object);
        }

        [Fact]
        public void GetCurrencyList()
        {
            //Arrange & Act
            IActionResult result = _controller.Get();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();
        }

        [Fact]
        public void Controller_Construction_Test()
        {
            //Assert
            Assert.NotNull(_controller);
        }
    }
}
