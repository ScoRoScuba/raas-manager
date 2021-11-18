using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using OFX.RAASManager.Core.Mongo;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Core.Mongo.Repositories;
using OFX.RAASManager.Entities;
using Xunit;

namespace OFX.RAASManager.UnitTests.Services
{
    public class AuditSummaryTests
    {
        private readonly Mock<IMongoDatabaseContext> _mockMongoDatabaseContext;
        private readonly Mock<IMongoCollection<BsonDocument>> _mockMongoCollection;
        private readonly IAuditSummaryRepository _auditSummaryRepository;

        public AuditSummaryTests()
        {
            _mockMongoDatabaseContext = new Mock<IMongoDatabaseContext>();
            _mockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();

            _mockMongoDatabaseContext.Setup(m => m.GetCollection(It.IsAny<string>())).Returns(_mockMongoCollection.Object);

            _auditSummaryRepository = new AuditSummaryRepository(_mockMongoDatabaseContext.Object, new MongoOptions()
            {
                ConnectionString = "ConnectionString",
                ScheduledDateTimesTableName = "ScheduledDateTimesTableName"
            });
        }

        [Fact]
        public void Match_ReturnsSummaries_ForGetStaticRatesDateTimeByCriteria()
        {
            //Arrange
            var mockAsyncCursor = new Mock<IAsyncCursor<BsonDocument>>();
            mockAsyncCursor.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            IList<BsonDocument> bsonDocuments = GetStaticRatesAuditSummaryBsonDocuments();
            mockAsyncCursor.Setup(a => a.Current).Returns(bsonDocuments);
            _mockMongoCollection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(), It.IsAny<FindOptions<BsonDocument>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockAsyncCursor.Object));

            //Act
            var results = _auditSummaryRepository.Match(new GetAuditSummariesCriteria(5));

            //Assert
            results.Count().Should().Be(bsonDocuments.Count);
        }

        [Fact]
        public void Match_ReturnsSummaries_ForGetAuditSummaryByCriteria()
        {
            //Arrange
            var mockAsyncCursor = new Mock<IAsyncCursor<BsonDocument>>();
            mockAsyncCursor.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            IList<BsonDocument> bsonDocuments = GetStaticRatesAuditSummaryBsonDocuments();
            mockAsyncCursor.Setup(a => a.Current).Returns(new[] { bsonDocuments.First() });
            _mockMongoCollection.Setup(m => m.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(), It.IsAny<FindOptions<BsonDocument>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockAsyncCursor.Object));

            //Act
            var results = _auditSummaryRepository.Match(new GetAuditSummaryByIdCriteria("5c33ef327d2057f64375ea62"));

            //Assert
            results.Should().NotBe(null);
        }

        private List<BsonDocument> GetStaticRatesAuditSummaryBsonDocuments()
        {
            var spotRateAudits = BsonValue.Create(GetSpotRatesAudit());

            return new List<BsonDocument>()
            {
                new BsonDocument{
                    { nameof(AuditSummary._id), new ObjectId("5c33ef327d2057f64375ea62")},
                    { nameof(AuditSummary.StartDateUTC), new DateTime(2012, 01, 24)},
                    { nameof(AuditSummary.StopDateUTC), new DateTime(2012, 01, 27)},
                    { nameof(AuditSummary.DurationInHours), 22},
                    { nameof(AuditSummary.UpdatedBy), "Administrator"},
                    { nameof(AuditSummary.SpotRateAuditSummaries), spotRateAudits},
                },
                new BsonDocument{
                    { nameof(AuditSummary._id), "5c33ef327d2057f64375ea72"},
                    { nameof(AuditSummary.StartDateUTC), new DateTime(2012, 12, 24)},
                    { nameof(AuditSummary.StopDateUTC), new DateTime(2012, 12, 27)},
                    { nameof(AuditSummary.DurationInHours), 12},
                    { nameof(AuditSummary.UpdatedBy), "Administrator"},
                    { nameof(AuditSummary.SpotRateAuditSummaries), spotRateAudits},
                },
                new BsonDocument{
                    { nameof(AuditSummary._id), "5c33ef327d2057f64375ea82"},
                    { nameof(AuditSummary.StartDateUTC), new DateTime(2012, 06, 24)},
                    { nameof(AuditSummary.StopDateUTC), new DateTime(2012, 06, 27)},
                    { nameof(AuditSummary.DurationInHours), 3},
                    { nameof(AuditSummary.UpdatedBy), "Administrator"},
                    { nameof(AuditSummary.SpotRateAuditSummaries), spotRateAudits},
                },
            };
        }

        private IEnumerable<BsonDocument> GetSpotRatesAudit()
        {
            return new List<SpotRateAudit>
            {
                new SpotRateAudit()
                {
                    Currency = "AUD",
                    Rate = 2.0725388601m,
                    Bid = 2.0691888601036269430051813472m,
                    Ask = 2.0758888601036269430051813472m,
                    Spread = 0.0067m,
                    Margin = 0.32328m
                },
                new SpotRateAudit()
                {
                    Currency = "NZD",
                    Rate = 1.0725388601m,
                    Bid = 1.0691888601036269430051813472m,
                    Ask = 1.0758888601036269430051813472m,
                    Spread = 0.0061m,
                    Margin = 0.12318m
                }
            }.Select(audit => audit.ToBsonDocument());
        }
    }
}
