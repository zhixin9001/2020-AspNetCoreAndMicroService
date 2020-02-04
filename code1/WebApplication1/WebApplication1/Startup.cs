using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WebApplication1.Extensions;

namespace WebApplication1
{
    public class Startup //约定的，不是接口
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // 可选方法，注册服务
        public void ConfigureServices(IServiceCollection services)
        {
            //IServiceCollection 服务容器 自带的IOC
            //services.AddControllers(); //API
            //services.AddControllersWithViews();  //MVC简化版
            //services.AddRazorPages();  //这两个加起来相对于addMvc

            //services.AddCors();
            //services.AddSingleton<>;
            //services.AddMessage(builder => builder.UserEmail()); //比较规范的服务封装

            //方法3.注册配置选项的服务
            //services.Configure<AppSetting>(_config);

            var config1 = new ConfigurationBuilder().AddJsonFile("config1.json").Build();
            services.Configure<AppSetting>(config1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //启动后执行
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppSetting> appOption)
        {
            var appSetting = new AppSetting();
            _config.Bind(appSetting);   //全部绑定

            var webSetting = new AppSetting();
            _config.GetSection("WebSetting").Bind(webSetting);  //部分绑定

            app.Run(async context =>
            {
                var connStr = _config["connectionString"];
                var c1 = _config["WebSetting:title"]; //通用方法，适合少量配置
                await context.Response.WriteAsync($"{appOption.Value.ConnectionString} - {appSetting.ConnectionString} - {connStr} -  {c1}");
            });

            app.Use(async (context, next) =>  //有next
            {
                await context.Response.WriteAsync("Middleware 1 begin \r\n");
                await next();
                await context.Response.WriteAsync("Middleware 1 end \r\n");
            });

            app.Use(async (context, next) =>  //有next
            {
                await context.Response.WriteAsync("Middleware 2 begin \r\n");
                await next();
                await context.Response.WriteAsync("Middleware 2 end \r\n");
            });

            app.Run(async context => //没有next，终端中间件，用来短路管道的，一般放在最后兜底
            {
                await context.Response.WriteAsync("hello core \r\n");
            });

            //封装自定义中间件
            app.UseMiddleware<TestMiddleware>();
            //进一步封装
            app.UseTest();

            app.UseStaticFiles();
            //中间件执行的顺序，就是添加的顺序

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            // env.IsDevelopment()
            //env.IsProduction()
            //env.IsStaging()

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>  //终结点
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
