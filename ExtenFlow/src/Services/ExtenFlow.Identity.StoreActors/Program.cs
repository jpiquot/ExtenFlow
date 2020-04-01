using Dapr.Actors.AspNetCore;

using ExtenFlow.Identity.Actors;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ExtenFlow.Identity.StoreActors
{
    /// <summary>
    /// The program class
    /// </summary>
    public static class Program
    {
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder
        //                .UseStartup<Startup>()
        //                .UseActors(actorRuntime =>
        //                {
        //                    actorRuntime.RegisterActor<RoleActor>();
        //                    actorRuntime.RegisterActor<RoleCollectionActor>();
        //                    actorRuntime.RegisterActor<RoleClaimsActor>();
        //                    actorRuntime.RegisterActor<RoleClaimsCollectionActor>();
        //                    actorRuntime.RegisterActor<UserActor>();
        //                    actorRuntime.RegisterActor<UserCollectionActor>();
        //                    actorRuntime.RegisterActor<UserClaimsActor>();
        //                    actorRuntime.RegisterActor<UserClaimsCollectionActor>();
        //                    actorRuntime.RegisterActor<UserLoginsActor>();
        //                    actorRuntime.RegisterActor<UserLoginsCollectionActor>();
        //                    actorRuntime.RegisterActor<UserRoleCollectionActor>();
        //                    actorRuntime.RegisterActor<UserTokensActor>();
        //                });
        //        });

        /// <summary>
        /// Creates a IWebHostBuilder.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>IWebHostBuilder instance.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            .UseActors(actorRuntime =>
            {
                actorRuntime.RegisterActor<RoleActor>();
                actorRuntime.RegisterActor<RoleCollectionActor>();
                actorRuntime.RegisterActor<RoleClaimsActor>();
                actorRuntime.RegisterActor<RoleClaimsCollectionActor>();
                actorRuntime.RegisterActor<UserActor>();
                actorRuntime.RegisterActor<UserCollectionActor>();
                actorRuntime.RegisterActor<UserClaimsActor>();
                actorRuntime.RegisterActor<UserClaimsCollectionActor>();
                actorRuntime.RegisterActor<UserLoginsActor>();
                actorRuntime.RegisterActor<UserLoginsCollectionActor>();
                actorRuntime.RegisterActor<UserRoleCollectionActor>();
                actorRuntime.RegisterActor<UserTokensActor>();
            });

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}