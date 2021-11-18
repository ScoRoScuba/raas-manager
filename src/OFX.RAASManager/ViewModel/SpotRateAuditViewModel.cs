using System.ComponentModel.DataAnnotations;

namespace OFX.RAASManager.ViewModel
{
    public class SpotRateAuditViewModel
    {
        public string Currency { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        public decimal? Rate { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        public decimal Bid { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00000000}")]
        public decimal Ask { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        public decimal Spread { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}")]
        public decimal Margin { get; set; }
    }
}
