using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LocationService
{
    public class Program
    {
		public static void Main(string[] args)
		{
			IConfiguration config = new ConfigurationBuilder()
							.AddCommandLine(args)
							.Build();

			Startup.Args = args;

			var host = new WebHostBuilder()
						.UseKestrel()
						.UseStartup<Startup>()
						.UseConfiguration(config)
						.Build();

			host.Run();
		}
	}
}
