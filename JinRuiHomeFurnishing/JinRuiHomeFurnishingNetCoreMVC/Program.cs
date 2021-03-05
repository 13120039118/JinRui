using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace JinRuiHomeFurnishingNetCoreMVC
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddEnvironmentVariables()
          .Build();

        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();  

            //每日实时日志
            Log.Logger = new LoggerConfiguration()
         .ReadFrom.Configuration(Configuration)
         .WriteTo.File(Path.Combine(@"log", "log.txt"), LogEventLevel.Information, rollingInterval: RollingInterval.Day)
         .CreateLogger();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
     WebHost.CreateDefaultBuilder(args).UseSerilog()
         .UseStartup<Startup>()
         .Build();


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
