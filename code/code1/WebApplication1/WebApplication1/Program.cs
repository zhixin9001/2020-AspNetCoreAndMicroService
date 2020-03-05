using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run(); //
            //CreateHostBuilder(args).Build().Start(); //异同
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            //默认配置
            //环境变量
            //命令行参数
            //加载应用程序
            //配置默认日志主键
            Host.CreateDefaultBuilder(args)
                //启用kestrel
                //调用这里面一些扩展方法，进行自定义配置
                .ConfigureWebHostDefaults(webBuilder =>  //web主机
                {


                    //两种配置方式
                    //1. 组件配置
                    //webBuilder.ConfigureKestrel((context,options)=>options.Limits.MaxRequestBodySize=1024);

                    //日志
                    webBuilder.ConfigureLogging(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Error);
                        builder.AddConsole().AddDebug();

                    });
                    //2. 主机配置项
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseStartup(Assembly.GetExecutingAssembly().FullName); //多环境 多startup时
                    //命令行>应用配置>硬编码>环境变量
                    webBuilder.UseUrls("http://*:5000");  //硬编码
                });
    }
}
