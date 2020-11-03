using Discord.WebSocket;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Interfaces;
using DungeonMaster.Library.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.ServiceLoaders
{
    internal sealed class MainServiceLoader : IServiceLoader
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddLogging(lb =>
            {
                lb.SetMinimumLevel(LogLevel.Information);
                lb.AddConsole();
                lb.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            });
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddSingleton<Config>(Config.BuildFromFile());
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<DungeonMasterClient>();
            services.AddScoped<GuildMaster>();
            services.AddTransient<ReactMenu>();
            services.AddTransient<ReactMenuItem>();
            services.AddScoped<AuditLogger>(p => p.GetService<GuildMaster>().auditLogger);
        }
    }
}
