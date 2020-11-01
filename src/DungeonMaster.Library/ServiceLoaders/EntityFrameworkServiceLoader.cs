using static Microsoft.Extensions.DependencyInjection.MySqlServiceCollectionExtensions;
using static Microsoft.Extensions.DependencyInjection.EntityFrameworkServiceCollectionExtensions;

using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DungeonMaster.Library.Database;
using Guppy.Attributes;


namespace DungeonMaster.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class EntityFrameworkServiceLoader : IServiceLoader
    {
        public void ConfigureServices(GuppyServiceCollection services)
        {
            services.AddEntityFrameworkMySql();
            services.AddDbContext<DungeonContext>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
