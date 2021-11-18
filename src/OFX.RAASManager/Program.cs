using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace OFX.RAASManager
{
    public class Program
    {
        private static IConfiguration _configuration;

        public static void Main(string[] args)
        {
            LoadConfiguration();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();

            Log.Information("Getting the motors running....");

            BuildWebHostBuilder(args).Run();
        }

        private static void LoadConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static IWebHost BuildWebHostBuilder(params string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddEventLog();
                })
                .UseConfiguration(_configuration)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
    }
}
