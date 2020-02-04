using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Extensions
{
    public static class MessageServiceExtension
    {
        public static void AddMessage(this IServiceCollection services,Action<MessageServiceBuilder> configure)
        {
            var builder = new MessageServiceBuilder(services);
            configure(builder);
        }
    }
}
