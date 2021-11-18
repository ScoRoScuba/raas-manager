using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using OFX.RAASManager.Core.Interfaces.Repositories;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;
using Serilog;

namespace OFX.RAASManager.Core.Mongo.Repositories
{
    public class PrimaryProviderRepository : IPrimaryProviderRepository
    {
        private readonly IMongoDatabaseContext _mongoDatabaseContext;
        private readonly MongoOptions _mongoOptions;
        private IMongoCollection<BsonDocument> _mongoCollection;
        private const string PRIMARY_RATE_PROVIDER = "PrimaryRateProvider";

        public PrimaryProviderRepository(IMongoDatabaseContext mongoDatabaseContext, MongoOptions mongoOptions)
        {
            _mongoDatabaseContext = mongoDatabaseContext;
            _mongoOptions = mongoOptions;

            _mongoCollection = _mongoDatabaseContext.GetCollection(_mongoOptions.PrimaryProviderTableName);

            CreateMongoCollectionIfNotExists();
        }

        private void CreateMongoCollectionIfNotExists()
        {
            if (!_mongoDatabaseContext.CollectionExists(_mongoOptions.PrimaryProviderTableName))
            {
                _mongoDatabaseContext.CreateCollection(_mongoOptions.PrimaryProviderTableName);
                _mongoCollection = _mongoDatabaseContext.GetCollection(_mongoOptions.PrimaryProviderTableName);
            }
        }

        public Task SetPrimaryProvider(string primaryProvider)
        {
            var taskToComplete = Task.WhenAll(UpdateAsync(primaryProvider));

            return taskToComplete;
        }

        private Task UpdateAsync(string primaryProvider)
        {
            var filter = Builders<BsonDocument>.Filter.Exists(PRIMARY_RATE_PROVIDER);
            var insertPrimaryProvider = CreatePrimaryProviderBsonDocument(primaryProvider);

            var updateOptions = new UpdateOptions { IsUpsert = true };

            return _mongoCollection.ReplaceOneAsync(filter, insertPrimaryProvider, updateOptions);
        }

        private BsonDocument CreatePrimaryProviderBsonDocument(string primaryProvider)
        {
            return new BsonDocument
            {
                {PRIMARY_RATE_PROVIDER, primaryProvider}
            };
        }
        public string GetPrimaryProvider()
        {
            try
            {
                var collection = _mongoCollection.Find(x => true);

                var primaryRateProvider = collection.SingleOrDefault().ToBsonDocument();

                if (primaryRateProvider == null) return null;

                return primaryRateProvider.GetValue(PRIMARY_RATE_PROVIDER).ToString();
            }
            catch (Exception exception)
            {
                Log.Error($"Failed to get Primary rate provider, {exception}");
                return null;
            }
        }
    }
}
