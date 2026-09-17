using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MSHost = Microsoft.Extensions.Hosting.Host;

namespace EC.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            MSHost.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}