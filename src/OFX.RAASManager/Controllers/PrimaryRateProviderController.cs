using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OFX.RAASManager.Core.Interfaces;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.ViewModel;
using Serilog;

namespace OFX.RAASManager.Controllers
{
    [Produces("application/json")]
    [Route("PrimaryRateProvider")]
    public class PrimaryRateProviderController : ControllerBase
    {
        private readonly IPrimaryRateProviderService _primaryRateProviderService;
        private readonly IHttpClient _httpClient;
        private readonly ILoggerService _logger;

        public PrimaryRateProviderController(IPrimaryRateProviderService primaryRateProviderService, IHttpClient httpClient, ILoggerService logger )
        {
            _primaryRateProviderService = primaryRateProviderService;
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(string provider)
        {
            string content;
            using (var reader = new StreamReader(Request.Body))
            {
                content = reader.ReadToEndAsync().Result;
            }

            try
            {
                if (HandleRequestContent(content))
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                _logger.Info("PrimaryRateProviderController:HandleRequestContent:UnexpectedBodyContent-{@Exception}", e);
                return BadRequest();
            }

            if (string.IsNullOrEmpty(provider))
            {
                return BadRequest();
            }

            _primaryRateProviderService.SetPrimaryRateProvider(provider);

            return Ok();
        }

        private bool HandleRequestContent(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var message = Amazon.SimpleNotificationService.Util.Message.ParseMessage(content);

                if (message.IsSubscriptionType)
                {
                    var result = _httpClient.GetStringAsync(message.SubscribeURL).Result;
                    return true;
                }
            }

            return false;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var provider = _primaryRateProviderService.GetPrimaryRateProvider();
            if (string.IsNullOrEmpty(provider))
            {
                return NotFound();
            }
            return Ok(new PrimaryRateProviderViewModel
            {
                PrimaryRateProvider = provider
            });
        }
    }
}

