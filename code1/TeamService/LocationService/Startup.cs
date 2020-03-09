using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocationService.Models;
using LocationService.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LocationService
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static string[] Args { get; set; } = new string[] { };
        private ILogger logger;
        private ILoggerFactory loggerFactory;

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = InitializeConfiguration();

            this.loggerFactory = loggerFactory;
            this.loggerFactory.AddConsole(LogLevel.Information);
            this.loggerFactory.AddDebug();

            this.logger = this.loggerFactory.CreateLogger("Startup");
        }

        internal static IConfigurationRoot InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);
            return builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var transient = true;
            if (Configuration.GetSection("transient") != null)
            {
                transient = bool.Parse(Configuration.GetSection("transient").Value);
            }
            if (transient)
            {
                services.AddScoped<ILocationRecordRepository, MemoryLocationRecordRepository>();
            }
            else
            {
                var connectionString = Configuration.GetSection("postgres:cstr").Value;
                services.AddEntityFrameworkNpgsql().AddDbContext<LocationDBContext>(options =>
                    options.UseNpgsql(connectionString));
                services.AddScoped<ILocationRecordRepository, LocationRecordRepository>();
            }
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
