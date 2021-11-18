using System;

namespace OFX.RAASManager.ViewModel
{
    public class DateTimeViewModel
    {
        public DateTime? StartDateUTC { get; set; }
        public int StartTimeUTCInMinutes { get; set; }
        public DateTime? StopDateUTC { get; set; }
        public int StopTimeUTCInMinutes { get; set; }
    }
}

