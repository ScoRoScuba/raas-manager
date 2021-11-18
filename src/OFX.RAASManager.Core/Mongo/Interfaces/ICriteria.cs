using MongoDB.Bson;
using MongoDB.Driver;

namespace OFX.RAASManager.Core.Mongo.Interfaces
{
    public interface ICriteria<out T>
    {
        T MatchFrom(IMongoCollection<BsonDocument> collection);
    }
}

