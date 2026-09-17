using Abp.AspNetCore.Dependency;
using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization; // Thêm thư viện này

namespace EC.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Ép hệ thống dùng InvariantCulture để tránh crash khi chạy Invariant Mode
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            CreateHostBuilder(args).Build().Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseCastleWindsor(IocManager.Instance.IocContainer);
        }
    }
}