using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineDemo
{
    public class ApplicationBuilder
    {
        private static readonly IList<Func<RequestDelegate, RequestDelegate>> _component = new List<Func<RequestDelegate, RequestDelegate>>();

        public ApplicationBuilder Use(Func<HttpContext, Func<Task>, Task> middleware)
        {
            return Use(next =>
            {
                return context =>
                {
                    Task SimpleNext() => next(context);
                    return middleware(context, SimpleNext);
                };
            });
        }

        public ApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _component.Add(middleware);
            return this;
        }

        public RequestDelegate Build()
        {
            RequestDelegate app = context =>
            {
                Console.WriteLine("Default middleware");
                return Task.CompletedTask;
            };

            foreach (var component in _component.Reverse())
            {
                app = component(app);
            }
            return app;
        }
    }
}
