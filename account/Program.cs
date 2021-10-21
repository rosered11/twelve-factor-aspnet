using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Common.Hosting;
using Steeltoe.Discovery.Client;

namespace twelve_factor_aspnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Add Steeltoe config server
                .AddConfigServer()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .AddDiscoveryClient()
                .UseCloudHosting(81);
    }
}
