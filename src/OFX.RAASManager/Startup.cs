using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OFX.RAASManager.Application.Services;
using OFX.RAASManager.Config;
using OFX.RAASManager.Core;
using OFX.RAASManager.Core.Interfaces;
using OFX.RAASManager.Core.Interfaces.Repositories;
using OFX.RAASManager.Core.Interfaces.Services;
using OFX.RAASManager.Core.Mongo.Interfaces;
using OFX.RAASManager.Core.Mongo.Repositories;
using OFX.RAASManager.Extensions;
using OFX.RAASManager.Services;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace OFX.RAASManager
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2); ;

            AddSingletonDIMappings(services);
            AddScopedDIMappings(services);

            services.AddJwtAuthentication(_configuration.GetSection("AuthServer").Get<AuthServerConfig>());
            services.AddCors();

            services
                .AddMvc(options =>
                {
                    var authenticatedUserPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().RequireScope("ALLAPI");

                    options.Filters.Add(new AuthorizeFilter(authenticatedUserPolicy.Build()));
                }).AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        AllowIntegerValues = false
                    });
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Static Rates API", Version = "v1" });
            });

            services.AddOptions();
        }

        private void AddScopedDIMappings(IServiceCollection services)
        {
            services.AddScoped<IAuditSummaryService, AuditSummaryService>();
            services.AddScoped<IStaticRatesService, StaticRatesService>();
            services.AddScoped<IPrimaryRateProviderService, PrimaryRateProviderService>();
            services.AddScoped<IDateTimeRepository, DateTimeRepository>();
            services.AddScoped<IPrimaryProviderRepository, PrimaryProviderRepository>();
            services.AddScoped<IAuditSummaryRepository, AuditSummaryRepository>();
            services.AddScoped<IStaticRatesSchedulerService, StaticRatesSchedulerService>();
        }

        private void AddSingletonDIMappings(IServiceCollection services)
        {
            services.AddSingleton(Serilog.Log.Logger);
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton(new HttpClient { Timeout = TimeSpan.FromSeconds(5)});
            services.AddSingleton<IHttpClient, RAASHttpClient>();
        
            services.AddSingleton<ICurrencyCodesService, CurrencyCodesService>();
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSingleton<IDayLightSavingService, DayLightSavingService>();

            services.AddMongoSingleton(_configuration);
            services.AddAutoMapperSingleton();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(config =>
            {
                config.AllowAnyHeader();
                config.AllowAnyOrigin();
                config.AllowAnyMethod();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger/ui";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Static Rates V1");
                options.DisplayOperationId();
            }
            );

            app.UseMvc(routes => routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new { controller = "Home", action = "Swagger" }
            ));
            app.UseStaticFiles();
        }
    }
}
