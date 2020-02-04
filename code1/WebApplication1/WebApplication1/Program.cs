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
            //CreateHostBuilder(args).Build().Start(); //��ͬ
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            //Ĭ������
            //��������
            //�����в���
            //����Ӧ�ó���
            //����Ĭ����־����
            Host.CreateDefaultBuilder(args)
                //����kestrel
                //����������һЩ��չ�����������Զ�������
                .ConfigureWebHostDefaults(webBuilder =>  //web����
                {


                    //�������÷�ʽ
                    //1. �������
                    //webBuilder.ConfigureKestrel((context,options)=>options.Limits.MaxRequestBodySize=1024);

                    //��־
                    webBuilder.ConfigureLogging(builder =>
                    {
                        builder.SetMinimumLevel(LogLevel.Error);
                        builder.AddConsole().AddDebug();

                    });
                    //2. ����������
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseStartup(Assembly.GetExecutingAssembly().FullName); //�໷�� ��startupʱ
                    //������>Ӧ������>Ӳ����>��������
                    webBuilder.UseUrls("http://*:5000");  //Ӳ����
                });
    }
}
