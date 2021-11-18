using System;
using System.Net.Http;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OFX.RAASManager.Config;

namespace OFX.RAASManager.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, AuthServerConfig authServerConfig)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.Authority = authServerConfig.Url;
                        options.JwtValidationClockSkew = TimeSpan.FromSeconds(authServerConfig.JwtValidationClockSkewInSeconds);
                        if (authServerConfig.AllowUntrustedCertificate)
                        {
                            options.RequireHttpsMetadata = false;
                            options.JwtBackChannelHandler = new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback = (sender, certificate, chain, errors) => true
                            };
                        }
                    });
        }
    }
}