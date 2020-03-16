using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventProcessor.Events;
using EventProcessor.Location;
using EventProcessor.Location.Redis;
using EventProcessor.Models;
using EventProcessor.Queues;
using EventProcessor.Queues.AMQP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace EventProcessor
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env/*, ILoggerFactory loggerFactory*/)
        {
            //loggerFactory.AddConsole();
            //loggerFactory.AddDebug();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();

            services.Configure<QueueOptions>(Configuration.GetSection("QueueOptions"));
            services.Configure<AMQPOptions>(Configuration.GetSection("amqp"));

            services.AddRedisConnectionMultiplexer(Configuration);

            services.AddTransient(typeof(IConnectionFactory), typeof(AMQPConnectionFactory));
            services.AddTransient(typeof(EventingBasicConsumer), typeof(AMQPEventingConsumer));

            services.AddSingleton(typeof(ILocationCache), typeof(RedisLocationCache));

            services.AddSingleton(typeof(IEventSubscriber), typeof(AMQPEventSubscriber));
            services.AddSingleton(typeof(IEventEmitter), typeof(AMQPEventEmitter));
            services.AddSingleton(typeof(IEventProcessor), typeof(MemberLocationEventProcessor));
        }

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    app.UseRouting();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapGet("/", async context =>
        //        {
        //            await context.Response.WriteAsync("Hello World!");
        //        });
        //    });
        //}

        public void Configure(IApplicationBuilder app,
        IWebHostEnvironment env,
        ILoggerFactory loggerFactory,
        IEventProcessor eventProcessor)
        {
            app.UseMvc();

            eventProcessor.Start();
        }
    }
}
