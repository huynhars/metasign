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

        public override void PostInitialize()
        {
            // Migrate DB TRƯỚC khi seed — DbContextOptions đã đăng ký xong ở PreInitialize
            using (var scope = IocManager.CreateScope())
            {
                var dbContext = scope.Resolve<ECDbContext>();
                dbContext.Database.Migrate();
            }

            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}