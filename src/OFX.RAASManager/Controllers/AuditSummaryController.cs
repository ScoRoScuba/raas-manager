using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Entities;
using OFX.RAASManager.ViewModel;

namespace OFX.RAASManager.Controllers
{
    public class AuditSummaryController : Controller
    {
        private const int DefaultMaxSummaryRecords = 5;

        private readonly IAuditSummaryService _auditSummaryService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuditSummaryController(IAuditSummaryService auditSummaryService, IMapper mapper, IConfiguration configuration)
        {
            _auditSummaryService = auditSummaryService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Summary()
        {
            var noOfRecordsToReturn = _configuration.GetValue("MaxSummaryRecords", DefaultMaxSummaryRecords);
            var auditList = _auditSummaryService.GetAuditSummaries(noOfRecordsToReturn);

            var viewModel = new AuditViewModel
            {
                AuditSummaries = ConvertToAuditSummaryViewModels(auditList)
            };

            return View(viewModel);
        }

        private IEnumerable<AuditSummaryViewModel> ConvertToAuditSummaryViewModels(IEnumerable<AuditSummary> staticRatesAuditList)
        {
            return _mapper.Map<IEnumerable<AuditSummaryViewModel>>(staticRatesAuditList);
        }

        private AuditSummaryViewModel ConvertToAuditSummaryViewModel(AuditSummary staticRatesAudit)
        {
            return _mapper.Map<AuditSummaryViewModel>(staticRatesAudit);
        }

        [AllowAnonymous]
        public IActionResult Rates(string auditSummaryId)
        {
            var spotRatesAuditSummary = _auditSummaryService.GetAuditSummary(auditSummaryId);
            AuditSummaryViewModel viewModel = ConvertToAuditSummaryViewModel(spotRatesAuditSummary);

            return View(viewModel);
        }
    }
}