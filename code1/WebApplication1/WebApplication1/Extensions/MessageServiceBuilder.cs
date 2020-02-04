using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Extensions
{
    public class MessageServiceBuilder
    {
        public IServiceCollection ServiceCollection { get; set; }

        public MessageServiceBuilder(IServiceCollection services)
        {
            ServiceCollection = services;
        }

        public void UserEmail()
        {

        }

        public void UserSms()
        {

        }
    }
}
