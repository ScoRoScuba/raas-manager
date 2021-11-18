using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Mongo
{
    public class GetAuditSummaryByIdCriteria : ICriteria<AuditSummary>
    {
        private readonly string _auditSummaryId;

        public GetAuditSummaryByIdCriteria(string auditSummaryId)
        {
            _auditSummaryId = auditSummaryId;
        }

        public AuditSummary MatchFrom(IMongoCollection<BsonDocument> collection)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(nameof(AuditSummary._id), new ObjectId(_auditSummaryId));

            using (IAsyncCursor<BsonDocument> cursor = collection.FindAsync(filter).Result)
            {
                return GetStaticRatesDateTimeFromCursor(cursor);
            };
        }

        private AuditSummary GetStaticRatesDateTimeFromCursor(IAsyncCursor<BsonDocument> cursor)
        {
            if (cursor.MoveNext())
            {
                IEnumerable<BsonDocument> documents = cursor.Current;

                return ConvertBsonDocumentToStaticRatesDateTime(documents);
            }

            return null;
        }

        private AuditSummary ConvertBsonDocumentToStaticRatesDateTime(IEnumerable<BsonDocument> documents)
        {
            return documents
                .Select(document => BsonSerializer.Deserialize<AuditSummary>(document))
                .SingleOrDefault();
        }
    }
}