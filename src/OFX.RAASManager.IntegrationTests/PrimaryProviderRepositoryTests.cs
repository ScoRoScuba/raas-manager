using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using OFX.RAASManager.Core.Interfaces.Repositories;
using OFX.RAASManager.Core.Mongo;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Core.Mongo.Repositories;
using OFX.RAASManager.Entities;
using Xunit;

namespace OFX.RAASManager.IntegrationTests
{
    public class PrimaryProviderRepositoryTests : IDisposable
    {
        private readonly IPrimaryProviderRepository _primaryProviderRepository;
        private readonly IMongoCollection<BsonDocument> _mongoCollection;
        private readonly IMongoDatabaseContext _mongoDatabaseContext;
        private readonly string _collectionName;
        private readonly List<BsonDocument> _primaryRateProviders;

        public PrimaryProviderRepositoryTests()
        {
            _collectionName = "primary_provider_test_" + DateTime.Now.Ticks;
            var connectionString = "mongodb://localhost:27017";

            _mongoDatabaseContext = new MongoDatabaseContext(new MongoClient(connectionString), "RAASManager");
            
            _primaryProviderRepository = new PrimaryProviderRepository(_mongoDatabaseContext, new MongoOptions(){
                ConnectionString = connectionString,
                PrimaryProviderTableName = _collectionName
            });

            _mongoCollection = _mongoDatabaseContext.GetCollection(_collectionName);
            _primaryRateProviders = new List<BsonDocument>
            {
                new BsonDocument{{ "PrimaryRateProvider",new BsonString("FIS")}},
                new BsonDocument{{ "PrimaryRateProvider",new BsonString("BAML")}}
            };
        }

        [Fact]

        public void SetPrimaryProviderSavedDocSuccessfully()
        {
            _primaryProviderRepository.SetPrimaryProvider("FIS");

            ReadFromCollection("FIS").Count.Should().Be(1);
        }

        private List<BsonDocument> ReadFromCollection(string providerName)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("PrimaryRateProvider", providerName);
            return _mongoCollection.FindSync(filter).ToList();
        }

        [Fact]
        public void ShouldGetPrimaryRateProvider()
        {
            _primaryProviderRepository.SetPrimaryProvider("FIS");
            Thread.Sleep(100);
            _primaryProviderRepository.GetPrimaryProvider().Should().Be("FIS");
        }

        [Fact]
        public void ShouldReturnNullWhenMoreThanOneRateProvider()
        {
            _mongoCollection.InsertMany(_primaryRateProviders);
            Thread.Sleep(100);
            _primaryProviderRepository.GetPrimaryProvider().Should().BeNull();
        }
        
        public void Dispose()
        {
            _mongoDatabaseContext.DropCollection(_collectionName);
        }
    }
}
