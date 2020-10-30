using Discord.WebSocket;
using DungeonMaster.Library.GuildManagers;
using Guppy;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Guppy.Lists;
using DungeonMaster.Library.ReactMenus;
using DungeonMaster.Library.ServiceLists;

namespace DungeonMaster.Library.ServiceLoaders
{
    /// <summary>
    /// The primaru service loader that will register all core
    /// DungeonMaster services.
    /// </summary>
    [AutoLoad]
    internal sealed class DungeonMasterServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddFactory<DungeonMasterClient>(p => new DungeonMasterClient());
            services.AddFactory<Config>(p => Config.BuildFromFile());
            services.AddFactory<DiscordSocketClient>(p => new DiscordSocketClient());
            services.AddFactory<PrimaryGuildManager>(p => new PrimaryGuildManager());
            services.AddFactory<StorageGuildManager>(p => new StorageGuildManager());
            services.AddFactory<ReactMenu>(p => new ReactMenu());
            services.AddFactory<ReactMenuItem>(p => new ReactMenuItem());
            services.AddFactory<ReactMenuList>(p => new ReactMenuList());
            services.AddFactory<AuditLog>(p => new AuditLog());

            services.AddSingleton<DungeonMasterClient>();
            services.AddSingleton<Config>();
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<PrimaryGuildManager>();
            services.AddSingleton<StorageGuildManager>();
            services.AddTransient<ReactMenu>();
            services.AddTransient<ReactMenuItem>();
            services.AddSingleton<ReactMenuList>(autoBuild: true);
            services.AddSingleton<AuditLog>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
