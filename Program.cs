using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SCMR_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // .ConfigureKestrel(serverOptions =>
                // {
                //     serverOptions.Limits.MaxRequestBodySize = null;
                //     serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromHours(2);
                //     serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromHours(3);
                // })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                    config.AddJsonFile("appsettings.json");
                    config.AddCommandLine(args);
                })
                .UseStartup<Startup>();
    }
}
