using Clients.GameClient;
using Common.Api.Middlewares;
using Common.Api.WebSocketManager;
using Identity.Api.Constants;
using Identity.Api.DataAccess;
using Identity.Api.Services;
using Identity.Api.WebSocketManager;
using Identity.Api.WebSocketManager.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Identity.Api
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            AddAuth(services);

            services.AddMvc();

            services.AddWebSocketManager();

            services.AddSingleton(typeof(UserRepository));
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IMembershipService, MembershipService>();
            services.AddSingleton(typeof(GameHttpClient));
            services.AddSingleton(typeof(RoomManager));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            UseAuth(app);
            app.UseMvc();
            app.UseWebSockets();

            WebSocketMessageArgsHandler.AddOrReplaceEvent<RoomAddedMessageArgs>(IdentityWebSocketEvent.RoomAdded);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<RoomRemovedMessageArgs>(IdentityWebSocketEvent.RoomRemoved);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<PlayerJoinedMessageArgs>(IdentityWebSocketEvent.PlayerJoined);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<PlayerLeftMessageArgs>(IdentityWebSocketEvent.PlayerLeft);

            app.MapWebSocketManager("/id", serviceProvider.GetService<IdentityHandler>());
        }
    }
}
