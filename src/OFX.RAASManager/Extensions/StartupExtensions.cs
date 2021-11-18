using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OFX.RAASManager.Core.Mongo;
using OFX.RAASManager.Core.Mongo.Config;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Entities;
using OFX.RAASManager.ViewModel;

namespace OFX.RAASManager.Extensions
{
    public static class StartupExtensions
    {
        public static void AddAutoMapperSingleton(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var config = new MapperConfiguration(configuration =>
                {
                    configuration.CreateMap<AuditSummary, AuditSummaryViewModel>()
                        .ForMember(destination => destination.Id, opts => opts.MapFrom(source => source._id));

                    configuration.CreateMap<SpotRateAudit, SpotRateAuditViewModel>();

                    configuration.CreateMap<StaticRateDateTime, DateTimeViewModel>();
                });

                return config.CreateMapper();
            });
        }

        public static void AddMongoSingleton(this IServiceCollection services, IConfiguration _configuration )
        {
            services.AddSingleton(new MongoOptionsBuilder(_configuration).Build());
            services.AddSingleton<IMongoDatabaseContext>(provider =>
            {
                var options = provider.GetService<MongoOptions>();
                return new MongoDatabaseContext(
                    new MongoClient(options.ConnectionString),
                    options.DatabaseName);
            });

        }
    }
}
