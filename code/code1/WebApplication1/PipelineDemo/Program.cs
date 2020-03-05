using System;
using System.Threading.Tasks;

namespace PipelineDemo
{

    public delegate Task RequestDelegate(HttpContext context);
    class Program
    {
        static void Main(string[] args)
        {
            var app = new ApplicationBuilder();

            app.Use(async (context,next)=>
            {
                Console.WriteLine("Middleware 1, begin");
                await next();
                Console.WriteLine("Middleware 1, end");
            });

            app.Use(async (context, next) =>
            {
                Console.WriteLine("Middleware 2, begin");
                await next();
                Console.WriteLine("Middleware 2, end");
            });

            var m = app.Build();
            m(new HttpContext());
        }
    }
}
