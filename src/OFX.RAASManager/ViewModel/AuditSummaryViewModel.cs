using System;
using System.Collections.Generic;

namespace OFX.RAASManager.ViewModel
{
    public class AuditSummaryViewModel
    {
        public string Id { get; set; }
        public DateTime StartDateUTC { get; set; }
        public DateTime StopDateUTC { get; set; }
        public int DurationInHours { get; set; }
        public string UpdatedBy { get; set; }
        public IEnumerable<SpotRateAuditViewModel> SpotRateAuditSummaries { get; set; }
        public string Status => GetStatus();

        private string GetStatus()
        {
            var utcNow = DateTime.UtcNow;
            if (StartDateUTC <= utcNow && StopDateUTC >= utcNow)
                return "ACTIVE";

            return "COMPLETE";
        }
    }
}
