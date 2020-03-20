using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
            .UseKestrel(op =>
            {
                op.Limits.MaxRequestBodySize = null;
                op.Limits.RequestHeadersTimeout = TimeSpan.FromHours(2);
                op.Limits.KeepAliveTimeout = TimeSpan.FromHours(3);
            }).UseStartup<Startup>();
    }
}
