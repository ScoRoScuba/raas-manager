using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using OFX.RAASManager.Core.Interfaces.Repositories;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Mongo.Repositories
{
    public class DateTimeRepository : IDateTimeRepository
    {
        private readonly IMongoDatabaseContext _mongoDatabaseContext;
        private readonly MongoOptions _mongoOptions;
        private IMongoCollection<BsonDocument> _mongoCollection;

        public DateTimeRepository(IMongoDatabaseContext mongoDatabaseContext, MongoOptions mongoOptions)
        {
            _mongoDatabaseContext = mongoDatabaseContext;
            _mongoOptions = mongoOptions;
            _mongoCollection = _mongoDatabaseContext.GetCollection(_mongoOptions.ScheduledDateTimesTableName);

            CreateMongoCollectionIfNotExists();
        }

        private void CreateMongoCollectionIfNotExists()
        {
            if (!_mongoDatabaseContext.CollectionExists(_mongoOptions.ScheduledDateTimesTableName))
            {
                _mongoDatabaseContext.CreateCollection(_mongoOptions.ScheduledDateTimesTableName);
                _mongoCollection = _mongoDatabaseContext.GetCollection(_mongoOptions.ScheduledDateTimesTableName);
            }
        }

        public T Match<T>(ICriteria<T> criteria)
        {
            return criteria.MatchFrom(_mongoCollection);
        }

        private Task InsertAsync(StaticRateDateTime staticRateDateTime)
        {
            var insertStaticRateDateTimes = CreateStaticRateDateTimeBsonDocument(staticRateDateTime);

            return _mongoCollection.InsertOneAsync(insertStaticRateDateTimes);
        }

        private static BsonDocument CreateStaticRateDateTimeBsonDocument(StaticRateDateTime staticRateDateTime)
        {
            if (staticRateDateTime.StartDateUTC == null)
            {
                return new BsonDocument
                {
                    {nameof(staticRateDateTime.StartTimeUTCInMinutes), staticRateDateTime.StartTimeUTCInMinutes},
                    {nameof(staticRateDateTime.StopTimeUTCInMinutes), staticRateDateTime.StopTimeUTCInMinutes }

                };

            }
            return new BsonDocument
            {
                {nameof(staticRateDateTime.StartDateUTC), staticRateDateTime.StartDateUTC },
                {nameof(staticRateDateTime.StartTimeUTCInMinutes), staticRateDateTime.StartTimeUTCInMinutes},
                {nameof(staticRateDateTime.StopDateUTC), staticRateDateTime.StopDateUTC},
                {nameof(staticRateDateTime.StopTimeUTCInMinutes), staticRateDateTime.StopTimeUTCInMinutes }

            };
        }

        public Task SetDateTime(IList<StaticRateDateTime> mongoStaticRateDateTimes)
        {
            var taskToComplete = Task.WhenAll(mongoStaticRateDateTimes.Select(InsertAsync));

            return taskToComplete;
        }

        public IList<StaticRateDateTime> GetDateTime()
        {
            var staticRatesDateTime = Match(new GetStaticRatesDateTimeByCriteria());

            return staticRatesDateTime;
        }
    }
}
