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
    public class Startup //Լ���ģ����ǽӿ�
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // ��ѡ������ע�����
        public void ConfigureServices(IServiceCollection services)
        {
            //IServiceCollection �������� �Դ���IOC
            //services.AddControllers(); //API
            //services.AddControllersWithViews();  //MVC�򻯰�
            //services.AddRazorPages();  //�����������������addMvc

            //services.AddCors();
            //services.AddSingleton<>;
            //services.AddMessage(builder => builder.UserEmail()); //�ȽϹ淶�ķ����װ

            //����3.ע������ѡ��ķ���
            //services.Configure<AppSetting>(_config);

            var config1 = new ConfigurationBuilder().AddJsonFile("config1.json").Build();
            services.Configure<AppSetting>(config1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //������ִ��
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppSetting> appOption)
        {
            var appSetting = new AppSetting();
            _config.Bind(appSetting);   //ȫ����

            var webSetting = new AppSetting();
            _config.GetSection("WebSetting").Bind(webSetting);  //���ְ�

            app.Run(async context =>
            {
                var connStr = _config["connectionString"];
                var c1 = _config["WebSetting:title"]; //ͨ�÷������ʺ���������
                await context.Response.WriteAsync($"{appOption.Value.ConnectionString} - {appSetting.ConnectionString} - {connStr} -  {c1}");
            });

            app.Use(async (context, next) =>  //��next
            {
                await context.Response.WriteAsync("Middleware 1 begin \r\n");
                await next();
                await context.Response.WriteAsync("Middleware 1 end \r\n");
            });

            app.Use(async (context, next) =>  //��next
            {
                await context.Response.WriteAsync("Middleware 2 begin \r\n");
                await next();
                await context.Response.WriteAsync("Middleware 2 end \r\n");
            });

            app.Run(async context => //û��next���ն��м����������·�ܵ��ģ�һ�������󶵵�
            {
                await context.Response.WriteAsync("hello core \r\n");
            });

            //��װ�Զ����м��
            app.UseMiddleware<TestMiddleware>();
            //��һ����װ
            app.UseTest();

            app.UseStaticFiles();
            //�м��ִ�е�˳�򣬾�����ӵ�˳��

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            // env.IsDevelopment()
            //env.IsProduction()
            //env.IsStaging()

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>  //�ս��
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
