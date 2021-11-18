using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using OFX.RAASManager.Core.Mongo.Interfaces;

namespace OFX.RAASManager.Core.Mongo
{
    public class MongoDatabaseContext : IMongoDatabaseContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoClient _mongoClient;
        private readonly string _databaseName;

        public MongoDatabaseContext(IMongoClient mongoClient, string databaseName)
        {
            _mongoClient = mongoClient;
            _databaseName = databaseName;
            _mongoDatabase = _mongoClient.GetDatabase(_databaseName);
        }

        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return _mongoDatabase.GetCollection<BsonDocument>(collectionName);
        }

        public void CreateCollection(string collectionName)
        {
            _mongoDatabase.CreateCollection(collectionName);
        }

        public void DropCollection(string collectionName)
        {
            _mongoDatabase.DropCollection(collectionName);
        }

        public bool CollectionExists(string collectionName)
        {
            return _mongoDatabase.ListCollections().ToList().Any(x => x.GetElement("name").Value.ToString() == collectionName);
        }
    }
}