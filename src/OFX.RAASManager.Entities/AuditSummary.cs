using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace OFX.RAASManager.Entities
{
    public class AuditSummary
    {
        public ObjectId _id { get; set; }
        public DateTime StartDateUTC { get; set; }
        public DateTime StopDateUTC { get; set; }
        public int DurationInHours { get; set; }
        public string UpdatedBy { get; set; }
        public IEnumerable<SpotRateAudit> SpotRateAuditSummaries { get; set; }
    }
}
