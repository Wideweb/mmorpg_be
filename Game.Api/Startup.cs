using Clients.IdentityClient;
using Common.Api.Middlewares;
using Common.Api.WebSocketManager;
using Game.Api.Constants;
using Game.Api.DataAccess;
using Game.Api.Services;
using Game.Api.WebSocketManager;
using Game.Api.WebSocketManager.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Game.Api
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
            services.AddCors();

            // Add framework services.
            AddAuth(services);
            services.AddMvc();

            services.AddWebSocketManager();
            
            services.AddSingleton(typeof(PlayerRepository));
            services.AddSingleton(typeof(DungeonRepository));
            services.AddSingleton(typeof(RoomManager));
            services.AddSingleton(typeof(IdentityHttpClient));
            services.AddTransient<IDungeonService, DungeonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseAuthentication();
            app.UseMvc();
            app.UseWebSockets();
            
            WebSocketMessageArgsHandler.AddOrReplaceEvent<GameObjectStateMessageArgs>(GameWebSocketEvent.GameObjectState);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<PlayerConnectedMessageArgs>(GameWebSocketEvent.PlayerConnected);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<PlayerDataMessageArgs>(GameWebSocketEvent.PlayerData);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<SetTargetMessageArgs>(GameWebSocketEvent.SetTarget);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<UseAbilityMessageArgs>(GameWebSocketEvent.UseAbility);
            WebSocketMessageArgsHandler.AddOrReplaceEvent<DealDamageMessageArgs>(GameWebSocketEvent.DealDamage);

            app.MapWebSocketManager("/gr", serviceProvider.GetService<GameRoomHandler>());
        }
    }
}
