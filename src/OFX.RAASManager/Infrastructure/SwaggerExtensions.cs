using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace OFX.RAASManager.Infrastructure
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            //add swagger integration
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "StaticRates API",
                    Description = "Description",
                    TermsOfService = "None"
                });
                options.DescribeAllEnumsAsStrings();
            });

            return services;
        }
    }
}
