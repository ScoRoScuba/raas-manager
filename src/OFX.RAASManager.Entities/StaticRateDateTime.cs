using System;

namespace OFX.RAASManager.Entities
{
    public class StaticRateDateTime
    {
        public DateTime? StartDateUTC { get; set; }
        public int StartTimeUTCInMinutes { get; set; }
        public DateTime? StopDateUTC { get; set; }
        public int StopTimeUTCInMinutes { get; set; }
    }
}
