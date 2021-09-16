using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database
{
    public sealed class DungeonContextFactory : IDesignTimeDbContextFactory<DungeonContext>
    {
        public DungeonContext CreateDbContext(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            DungeonSetup.ConfigureServices(services, args[0]);

            IServiceProvider provider = services.BuildServiceProvider();

            return provider.GetService<DungeonContext>();
        }
    }
}
