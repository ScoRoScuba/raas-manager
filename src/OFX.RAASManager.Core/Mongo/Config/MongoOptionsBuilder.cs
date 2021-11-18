using Microsoft.Extensions.Configuration;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Mongo.Config
{
    public class MongoOptionsBuilder : IConfigurationBuilder<MongoOptions>
    {
        private readonly IConfiguration _configuration;

        public MongoOptionsBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MongoOptions Build()
        {
            return new MongoOptions()
            {
                ConnectionString = _configuration["Mongo:ConnectionString"],
                ScheduledDateTimesTableName = _configuration["Mongo:ScheduledDateTimesTableName"],
                AuditSummaryTableName = _configuration["Mongo:AuditSummaryTableName"],
                DatabaseName = _configuration["Mongo:DatabaseName"],
                PrimaryProviderTableName = _configuration["Mongo:PrimaryProviderTableName"]
            };
        }
    }
}
