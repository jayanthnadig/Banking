using ASNRTech.CoreService.Utilities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASNRTech.CoreService.Config
{
    public class Program
    {
        public static IConfiguration Configuration;

        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            // start scheduler
            // Scheduler.StartAsync().ConfigureAwait(false);

            host.Run();
        }

        public static void BuildConfiguration()
        {
            Configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Utility.Environment}.json", optional: true, reloadOnChange: true)
              .Build();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            SetEbConfig();
            BuildConfiguration();

            return WebHost
               .CreateDefaultBuilder(args)
               .UseConfiguration(Configuration)
               .UseStartup<Startup>();
        }

        private static void SetEbConfig()
        {
            ConfigurationBuilder tempConfigBuilder = new ConfigurationBuilder();

            tempConfigBuilder.AddJsonFile(@"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration", optional: true, reloadOnChange: true);

            var configuration = tempConfigBuilder.Build();

            Dictionary<string, string> ebEnv = configuration
               .GetSection("iis:env")
               .GetChildren()
               .Select(pair => pair.Value.Split(new[] { '=' }, 2))
               .ToDictionary(keypair => keypair[0], keypair => keypair[1]);

            foreach (var keyVal in ebEnv)
            {
                Environment.SetEnvironmentVariable(keyVal.Key, keyVal.Value);
            }
        }
    }
}
