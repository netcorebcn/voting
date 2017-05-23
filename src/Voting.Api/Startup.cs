using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EasyEventSourcing;
using EasyWebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Voting.Domain;
using Voting.Domain.Events;

namespace Voting.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer().AddJsonFormatters();
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info { Title = "Voting API", Version = "v1" })
            );

            services.AddEasyEventSourcing(
                EventStoreOptions.Create(
                    Configuration["EVENT_STORE"],
                    Configuration["EVENT_STORE_MANAGER_HOST"],
                    Configuration["STREAM_NAME"]),
                ReflectionHelper.DomainAssembly);

            services.AddWebSocketManager();
            services.AddSingleton<VotingReadModelService>();
        }

        public void Configure(IApplicationBuilder app,
            IEventStoreBus eventBus,
            IEventStoreProjections projections,
            WebSocketHandler wsHandler,
            VotingReadModelService readModelService,
            ILogger<Startup> logger)
        {
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voting API"));

            projections.CreateAsync(Projections.Voting)
                .DefaultRetryAsync()
                .Wait();


            eventBus.Subscribe(
                async (@event) =>
                {                    
                    var snapshot = await readModelService.AddOrUpdate(@event);
                    logger.LogInformation(snapshot.ToString());
                    await wsHandler.SendMessageToAllAsync(snapshot);
                })
                .DefaultRetryAsync()
                .Wait();
        }
    }
}
