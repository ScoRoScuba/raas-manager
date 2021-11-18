namespace OFX.RAASManager.Entities
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string ScheduledDateTimesTableName { get; set; }

        public string AuditSummaryTableName { get; set; }

        public string PrimaryProviderTableName { get; set; }
    }
}
