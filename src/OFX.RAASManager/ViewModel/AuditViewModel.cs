using System.Collections.Generic;

namespace OFX.RAASManager.ViewModel
{
    public class AuditViewModel
    {
        public IEnumerable<AuditSummaryViewModel> AuditSummaries { get; set; }
    }
}
