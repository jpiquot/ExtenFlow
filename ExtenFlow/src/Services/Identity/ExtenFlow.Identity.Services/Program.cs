using System;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ExtenFlow.Identity.StoreActors
{
    /// <summary>
    /// The program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Creates a IWebHostBuilder.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>IWebHostBuilder instance.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            string? aspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationRoot currentConfig = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json")
                    .AddJsonFile($"appsettings.{aspnetcoreEnvironment}.json")
                    .Build();

            return WebHost
                .CreateDefaultBuilder(args)
                    .UseConfiguration(currentConfig)
                    .UseStartup<Startup>();
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}