using System;

using Dapr.Actors.AspNetCore;

using ExtenFlow.Identity.Roles.Application.Helpers;
using ExtenFlow.Messages.Events;

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
        public static IHostBuilder CreateWebBuilder(string[] args)
        {
            string? aspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationRoot currentConfig = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json")
                    .AddJsonFile($"appsettings.{aspnetcoreEnvironment}.json")
                    .Build();

            return Host
                    .CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseConfiguration(currentConfig);
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseActors(actorRuntime =>
                        {
                            actorRuntime.RegisterRoleActors(
                                () => (new InMemoryEventPublisherBuilder()).Build(),
                                (name) => (new InMemoryEventStoreBuilder()).Name(name).Build());
                            //actorRuntime.RegisterUserActors();
                        });
                    });
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
            => CreateWebBuilder(args).Build().Run();
    }
}