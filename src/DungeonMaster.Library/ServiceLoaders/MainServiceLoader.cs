using Discord.WebSocket;
using DungeonMaster.Library.Database;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class MainServiceLoader : IServiceLoader
    {
        public void ConfigureServices(GuppyServiceCollection services)
        {
            services.AddLogging(lb =>
            {
                lb.AddConsole();
            });

            services.AddSingleton<Config>(Config.BuildFromFile());
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<DungeonMasterClient>();
            services.AddScoped<GuildMaster>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
