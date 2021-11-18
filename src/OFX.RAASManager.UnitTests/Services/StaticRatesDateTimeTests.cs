using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using OFX.RAASManager.Core.Interfaces.Repositories;
using OFX.RAASManager.Core.Mongo;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Core.Mongo.Repositories;
using OFX.RAASManager.Entities;
using Xunit;

namespace OFX.RAASManager.UnitTests.Services
{
    public class StaticRatesDateTimeTests
    {
        private readonly Mock<IMongoDatabaseContext> _mockMongoDatabaseContext;
        private readonly Mock<IMongoCollection<BsonDocument>> _mockMongoCollection;
        private  IDateTimeRepository _dateTimeRepository;

        public StaticRatesDateTimeTests()
        {
            _mockMongoDatabaseContext = new Mock<IMongoDatabaseContext>();
            _mockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();

            _mockMongoDatabaseContext.Setup(m => m.GetCollection(It.IsAny<string>())).Returns(_mockMongoCollection.Object);

            _mockMongoDatabaseContext.Setup(m => m.CollectionExists(It.IsAny<string>())).Returns(false);

            _dateTimeRepository = new DateTimeRepository(_mockMongoDatabaseContext.Object, new MongoOptions()
            {
                ConnectionString = "ConnectionString",
                ScheduledDateTimesTableName = "ScheduledDateTimesTableName"
            });
        }

        [Fact]
        public void GetStaticRateDateTime()
        {
            IList<BsonDocument> bsonDocuments = new List<BsonDocument>()
            {
                new BsonDocument{
                    { nameof(StaticRateDateTime.StartDateUTC), new DateTime(2012, 12, 24)},
                    { nameof(StaticRateDateTime.StartTimeUTCInMinutes), 1320 },
                    { nameof(StaticRateDateTime.StopDateUTC), new DateTime(2012, 12, 27)},
                    { nameof(StaticRateDateTime.StopTimeUTCInMinutes), 1200}
                },
                new BsonDocument{
                    { nameof(StaticRateDateTime.StartDateUTC), new DateTime(2018, 12, 31) },
                    { nameof(StaticRateDateTime.StartTimeUTCInMinutes),1320 },
                    { nameof(StaticRateDateTime.StopDateUTC), new DateTime(2018, 01, 02 )},
                    { nameof(StaticRateDateTime.StopTimeUTCInMinutes), 540}
                }
            };

            var mockAsyncCursor = new Mock<IAsyncCursor<BsonDocument>>();

            mockAsyncCursor.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            mockAsyncCursor.Setup(a => a.Current).Returns(bsonDocuments);

            _mockMongoCollection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(), It.IsAny<FindOptions<BsonDocument>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockAsyncCursor.Object));

            var results = _dateTimeRepository.Match(new GetStaticRatesDateTimeByCriteria());

            results.Count.Should().Be(bsonDocuments.Count);
        }
    }
}
