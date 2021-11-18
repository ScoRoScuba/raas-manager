using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Mongo
{
    public class GetAuditSummariesCriteria : ICriteria<IEnumerable<AuditSummary>>
    {
        private readonly int _numberOfSummaries;

        public GetAuditSummariesCriteria(int numberOfSummaries)
        {
            _numberOfSummaries = numberOfSummaries;
        }

        public IEnumerable<AuditSummary> MatchFrom(IMongoCollection<BsonDocument> collection)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var options = GetFindOptions();

            using (IAsyncCursor<BsonDocument> cursor = collection.FindAsync(filter, options).Result)
            {
                return GetStaticRatesDateTimeFromCursor(cursor);
            }
        }

        private FindOptions<BsonDocument> GetFindOptions()
        {
            return new FindOptions<BsonDocument>
            {
                Projection = Builders<BsonDocument>.Projection.Exclude(nameof(AuditSummary.SpotRateAuditSummaries)),
                Sort = Builders<BsonDocument>.Sort.Descending(nameof(AuditSummary.StartDateUTC)),
                Limit = _numberOfSummaries
            };
        }

        private IEnumerable<AuditSummary> GetStaticRatesDateTimeFromCursor(IAsyncCursor<BsonDocument> cursor)
        {
            var staticRatesDateTime = new List<AuditSummary>();
            while (cursor.MoveNext())
            {
                IEnumerable<BsonDocument> documents = cursor.Current;

                staticRatesDateTime.AddRange(ConvertBsonDocumentToStaticRatesDateTime(documents));
            }

            return staticRatesDateTime;
        }

        private IEnumerable<AuditSummary> ConvertBsonDocumentToStaticRatesDateTime(IEnumerable<BsonDocument> documents)
        {
            return documents
                .Select(document => BsonSerializer.Deserialize<AuditSummary>(document))
                .ToList();
        }
    }
}
