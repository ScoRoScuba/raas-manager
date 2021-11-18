using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OFX.RAASManager.Controllers;
using OFX.RAASManager.Core.Interfaces;
using OFX.RAASManager.Core.Interfaces.Services;
using Xunit;

namespace OFX.RAASManager.API.IntegrationTests.Controller
{
    public class PrimaryRateFeedControllerTests
    {
        private Mock<IPrimaryRateProviderService> _mockPrimaryRateFeedProviderService;
        private Mock<IHttpClient> _mockHttpClientService;
        private Mock<ILoggerService> _mockLoggerService;

        private IWebHostBuilder _webHostBuilder;
        private TestServer _testServer;
        private HttpClient _httpTestClient;

        private readonly string RequestWithoutProvider = "/PrimaryRateProvider";
        private readonly string RequestWithProvider = "/PrimaryRateProvider?provider=testprovider";

        public PrimaryRateFeedControllerTests()
        {
            _mockPrimaryRateFeedProviderService = new Mock<IPrimaryRateProviderService>();
            _mockHttpClientService = new Mock<IHttpClient>();
            _mockLoggerService = new Mock<ILoggerService>();

            _webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup<PrimaryRateProviderController>>() // Uses Start up class from your API Host project to configure the test server
                .ConfigureTestServices(srv =>
                {
                    srv.AddSingleton(_mockPrimaryRateFeedProviderService.Object);
                    srv.AddSingleton(_mockHttpClientService.Object);
                    srv.AddSingleton(_mockLoggerService.Object);
                });

            _testServer = new TestServer( _webHostBuilder);
            _httpTestClient = _testServer.CreateClient();
        }

        [Fact]
        public void WhenPostedAWSSubscriptionMessageReturnsHttpOkResponse()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.HttpSubscriptionConfirmation;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            Assert.True( result.IsSuccessStatusCode );
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void WhenPostedAWSSubscriptionMessageUsesGetStringAsyncToSubscriptionUrl()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.HttpSubscriptionConfirmation;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            _mockHttpClientService.Verify(h => h.GetStringAsync(AWSSNSMessageTypes.SubscriptionUrl), Times.AtLeastOnce);
        }

        [Fact]
        public void WhenPostedAMessageWithoutQueryStringThatsNotAWSSubscritionReturnsBadRequest()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.NonsenseJson;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public void WhenPostedAMessageThatsNotAWSSubscritionInformationMessageShouldBeLogged()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.NonsenseJson;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            _mockLoggerService.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<Exception>()), Times.AtLeastOnce);
        }

        [Fact]
        public void WhenPostingWithAMessageBodyShoudNotUsePrimaryRateProviderService()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.HttpSubscriptionConfirmation;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            _mockPrimaryRateFeedProviderService.Verify(p => p.SetPrimaryRateProvider(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void WhenPostingWithAWSMessageBodyWithQueryStringShoudNotUsePrimaryRateProviderService()
        {
            var requestUrl = RequestWithProvider;

            var postData = AWSSNSMessageTypes.HttpSubscriptionConfirmation;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            _mockPrimaryRateFeedProviderService.Verify(p => p.SetPrimaryRateProvider(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void WhenPostingWithoutAMessageBodyAndNoQueryStringShoudNotUsePrimaryRateProviderService()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.HttpSubscriptionConfirmation;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            _mockPrimaryRateFeedProviderService.Verify(p => p.SetPrimaryRateProvider(It.IsAny<string>()), Times.Never());
        }


        [Fact]
        public void WhenPostingWithANonsenseMessageBodyAndQueryStringShoudReturnBadRequest()
        {
            var requestUrl = RequestWithProvider;

            var postData = AWSSNSMessageTypes.NonsenseJson;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public void WhenPostingWithANonsenseMessageBodyAndQueryStringShoudNotUsePrimaryRateProviderService()
        {
            var requestUrl = RequestWithProvider;

            var postData = AWSSNSMessageTypes.NonsenseJson;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            _mockPrimaryRateFeedProviderService.Verify(p => p.SetPrimaryRateProvider(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void WhenPostingWithAMessageBodyAndNoQueryStringShoudGetBadRequestResponse()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.NonsenseJson;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            Assert.False(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public void WhenPostingWithAMessageBodyAndNoQueryStringShoudNotUsePrimaryRateProviderService()
        {
            var requestUrl = RequestWithoutProvider;

            var postData = AWSSNSMessageTypes.NonsenseJson;

            var httpContent = new StringContent(postData, Encoding.UTF8, "text/plain");

            var result = _httpTestClient.PostAsync(requestUrl, httpContent).Result;

            _mockPrimaryRateFeedProviderService.Verify(p => p.SetPrimaryRateProvider(It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public void WhenPostingWithoutAMessageBodyWithQueryStringShoudReturnOkResponse()
        {
            var requestUrl = RequestWithProvider;

            var result = _httpTestClient.PostAsync(requestUrl, null).Result;

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void WhenPostingWithoutAMessageBodyWithQueryStringShoudUsePrimaryRateProviderService()
        {
            var requestUrl = RequestWithProvider;
            
            var result = _httpTestClient.PostAsync(requestUrl, null).Result;

            _mockPrimaryRateFeedProviderService.Verify(p => p.SetPrimaryRateProvider(It.IsAny<string>()), Times.AtLeastOnce);
        }

    }
}
