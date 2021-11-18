using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Mongo
{
    public class GetStaticRatesDateTimeByCriteria : ICriteria<IList<StaticRateDateTime>>
    {
        public IList<StaticRateDateTime> MatchFrom(IMongoCollection<BsonDocument> collection)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var options = GetFindOptions();

            using (IAsyncCursor<BsonDocument> cursor = collection.FindAsync(filter, options).Result)
            {
                return GetStaticRatesDateTimeFromCursor(cursor);
            };
        }

        private FindOptions<BsonDocument> GetFindOptions()
        {
            return new FindOptions<BsonDocument>
            {
                Projection = Builders<BsonDocument>.Projection
                    .Include(nameof(StaticRateDateTime.StartDateUTC))
                    .Include(nameof(StaticRateDateTime.StartTimeUTCInMinutes))
                    .Include(nameof(StaticRateDateTime.StopDateUTC))
                    .Include(nameof(StaticRateDateTime.StopTimeUTCInMinutes))
                    .Exclude("_id")
            };
        }

        private IList<StaticRateDateTime> GetStaticRatesDateTimeFromCursor(IAsyncCursor<BsonDocument> cursor)
        {
            var staticRatesDateTime = new List<StaticRateDateTime>();
            while (cursor.MoveNext())
            {
                IEnumerable<BsonDocument> documents = cursor.Current;

                staticRatesDateTime.AddRange(ConvertBsonDocumentToStaticRatesDateTime(documents));
            }

            return staticRatesDateTime;
        }

        private IList<StaticRateDateTime> ConvertBsonDocumentToStaticRatesDateTime(IEnumerable<BsonDocument> documents)
        {
            return documents
                .Select(document => BsonSerializer.Deserialize<StaticRateDateTime>(document))
                .ToList();
        }
    }
}
