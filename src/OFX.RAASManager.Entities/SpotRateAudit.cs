namespace OFX.RAASManager.Entities
{
    public class SpotRateAudit
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Spread { get; set; }
        public decimal Margin { get; set; }
    }
}
