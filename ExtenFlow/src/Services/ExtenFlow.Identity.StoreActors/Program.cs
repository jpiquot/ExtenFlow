using System;

using Dapr.Actors.AspNetCore;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Actors;

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
        private const int _appChannelHttpPort = 3000;

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
                    .UseStartup<Startup>()
                    .UseActors(actorRuntime =>
                    {
                        actorRuntime.RegisterActor<RoleActor>(information => new ActorService(information, (service, id) => new RoleActor(service, id)));
                        actorRuntime.RegisterActor<RoleCollectionActor>(information => new ActorService(information, (service, id) => new RoleCollectionActor(service, id)));
                        actorRuntime.RegisterActor<RoleClaimsActor>(information => new ActorService(information, (service, id) => new RoleClaimsActor(service, id)));
                        actorRuntime.RegisterActor<RoleClaimsCollectionActor>(information => new ActorService(information, (service, id) => new RoleClaimsCollectionActor(service, id)));
                        actorRuntime.RegisterActor<UserActor>(information => new ActorService(information, (service, id) => new UserActor(service, id)));
                        actorRuntime.RegisterActor<UserCollectionActor>(information => new ActorService(information, (service, id) => new UserCollectionActor(service, id)));
                        actorRuntime.RegisterActor<UserClaimsActor>(information => new ActorService(information, (service, id) => new UserClaimsActor(service, id)));
                        actorRuntime.RegisterActor<UserClaimsCollectionActor>(information => new ActorService(information, (service, id) => new UserClaimsCollectionActor(service, id)));
                        actorRuntime.RegisterActor<UserLoginsActor>(information => new ActorService(information, (service, id) => new UserLoginsActor(service, id)));
                        actorRuntime.RegisterActor<UserLoginsCollectionActor>(information => new ActorService(information, (service, id) => new UserLoginsCollectionActor(service, id)));
                        actorRuntime.RegisterActor<UserRoleCollectionActor>(information => new ActorService(information, (service, id) => new UserRoleCollectionActor(service, id)));
                        actorRuntime.RegisterActor<UserTokensActor>(information => new ActorService(information, (service, id) => new UserTokensActor(service, id)));
                    })
                    .UseUrls($"http://localhost:{_appChannelHttpPort}/");
        }
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();
    }
}