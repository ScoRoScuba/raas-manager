using System.Collections.Generic;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Interfaces.Services
{
    public interface IAuditSummaryService
    {
        IEnumerable<AuditSummary> GetAuditSummaries(int numberOfSummaries);
        AuditSummary GetAuditSummary(string auditSummaryId);
    }
}