using System.Collections.Generic;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Core.Mongo;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Application.Services
{
    public class AuditSummaryService : IAuditSummaryService
    {
        private readonly IAuditSummaryRepository _auditSummaryRepository;

        public AuditSummaryService(IAuditSummaryRepository auditSummaryRepository)
        {
            _auditSummaryRepository = auditSummaryRepository;
        }

        public IEnumerable<AuditSummary> GetAuditSummaries(int numberOfSummaries)
        {
            return _auditSummaryRepository.Match(new GetAuditSummariesCriteria(numberOfSummaries));
        }

        public AuditSummary GetAuditSummary(string auditSummaryId)
        {
            return _auditSummaryRepository.Match(new GetAuditSummaryByIdCriteria(auditSummaryId));
        }
    }
}
