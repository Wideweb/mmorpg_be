using Game.Api.DataAccess;
using Game.Api.Game.Services;
using Game.Api.Services;
using Game.Api.WebSocketManager;
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
            services.AddSingleton(typeof(UserRepository));
            services.AddSingleton(typeof(DungeonRepository));
            services.AddSingleton(typeof(RoomManager));
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<IDungeonService, DungeonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            UseAuth(app);

            app.UseMvc();

            app.UseWebSockets();

            app.MapWebSocketManager("/gr", serviceProvider.GetService<GameRoomHandler>());
        }
    }
}
