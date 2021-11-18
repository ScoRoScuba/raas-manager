using MongoDB.Bson;
using MongoDB.Driver;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;

namespace OFX.RAASManager.Core.Mongo.Repositories
{
    public class AuditSummaryRepository : IAuditSummaryRepository
    {
        private readonly IMongoDatabaseContext _mongoDatabaseContext;
        private readonly MongoOptions _mongoOptions;
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public AuditSummaryRepository(IMongoDatabaseContext mongoDatabaseContext, MongoOptions mongoOptions)
        {
            _mongoDatabaseContext = mongoDatabaseContext;
            _mongoOptions = mongoOptions;
            _mongoCollection = _mongoDatabaseContext.GetCollection(_mongoOptions.AuditSummaryTableName);
        }

        public T Match<T>(ICriteria<T> criteria)
        {
            return criteria.MatchFrom(_mongoCollection);
        }
    }
}