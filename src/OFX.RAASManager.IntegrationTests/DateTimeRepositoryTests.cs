using System;
using FluentAssertions;
using MongoDB.Driver;
using OFX.RAASManager.Core.Mongo;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Core.Mongo.Repositories;
using OFX.RAASManager.Entities;
using Xunit;

namespace OFX.RAASManager.IntegrationTests
{
    public class DateTimeRepositoryTests : IDisposable
    {
        private const string MongoDbConnectionString = "mongodb://localhost:27017";
        private const string TestDatabaseName = "DataBase_StaticRates";
        private const string TestCollectionName = "colleciton_DateTimes";
        private MongoClient _mongoClient;
        private readonly IMongoDatabaseContext _mongoDatabaseContext;
        private readonly MongoOptions mongoOptions;

        public DateTimeRepositoryTests()
        {
            _mongoClient = new MongoClient(MongoDbConnectionString);
            _mongoDatabaseContext = new MongoDatabaseContext(_mongoClient, TestDatabaseName);
            mongoOptions = new MongoOptions
            {
                ConnectionString = MongoDbConnectionString,
                DatabaseName = TestDatabaseName,
                ScheduledDateTimesTableName = TestCollectionName
            };
        }

        [Fact]
        public void CreateCollectionWithoutException()
        {
            Action action = () => new DateTimeRepository(_mongoDatabaseContext, mongoOptions);

            action.Should().NotThrow<Exception>();
        }
        public void Dispose()
        {
            _mongoClient.DropDatabase(TestDatabaseName);
        }
    }
}
