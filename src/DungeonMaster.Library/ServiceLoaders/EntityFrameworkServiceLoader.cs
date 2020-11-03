using System;
using System.Collections.Generic;
using System.Text;
using DungeonMaster.Library.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DungeonMaster.Library.Interfaces;

namespace DungeonMaster.Library.ServiceLoaders
{
    internal sealed class EntityFrameworkServiceLoader : IServiceLoader
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddEntityFrameworkMySql();
            services.AddDbContext<DungeonContext>();
            services.AddSingleton<DbContextOptions<DungeonContext>>(p =>
            {
                return new DbContextOptionsBuilder<DungeonContext>()
                    .UseMySql(p.GetService<Config>().ConnectionInfo.ToString())
                    .UseInternalServiceProvider(p)
                    .Options;
            });
        }
    }
}
