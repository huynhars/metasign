using Abp.Dependency;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using EC.EntityFrameworkCore.Seed;
using Microsoft.EntityFrameworkCore;

namespace EC.EntityFrameworkCore
{
    [DependsOn(
        typeof(ECCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class ECEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<ECDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        ECDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        ECDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ECEntityFrameworkModule).GetAssembly());
        }

        // Migrate + Seed KHÔNG còn ở đây. DbContextOptions<ECDbContext> chưa
        // resolve được ổn định trong lúc PostInitialize của các module đang chạy
        // (giữa Windsor / AspNetCore DI sync). Cả 2 việc chuyển sang
        // EC.Web.Host/Startup/Startup.cs, chạy sau khi app.UseAbp() hoàn tất.
    }
}