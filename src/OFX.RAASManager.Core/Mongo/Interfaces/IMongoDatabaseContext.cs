using MongoDB.Bson;
using MongoDB.Driver;

namespace OFX.RAASManager.Core.Mongo.Interfaces
{
    public interface IMongoDatabaseContext
    {
        IMongoCollection<BsonDocument> GetCollection(string collectionName);
        void CreateCollection(string collectionName);
        void DropCollection(string collectionName);
        bool CollectionExists(string collectionName);
    }
}