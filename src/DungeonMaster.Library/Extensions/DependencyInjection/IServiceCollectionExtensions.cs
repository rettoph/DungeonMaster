using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static void AddSingletonHostedService<THostedService>(this IServiceCollection services)
            where THostedService : class, IHostedService
        {
            services.AddSingleton<THostedService>();
            services.AddHostedService<THostedService>(p => p.GetService<THostedService>());

        }
    }
}
