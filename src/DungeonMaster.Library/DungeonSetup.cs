using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Extensions.DependencyInjection;
using DungeonMaster.Library.Services;
using DungeonMaster.Library.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DungeonMaster.Library
{
    public static class DungeonSetup
    {
        private static Boolean _configured = false;

        public static void ConfigureServices(
            IServiceCollection services, 
            String connectionString,
            String youtubeApplicationName,
            String youtubeKey,
            String ffmpeg,
            String youtubeDl,
            String libOpus,
            String libSodium)
        {
            if (_configured)
                return;

            services.AddDbContext<DungeonContext>(options =>
            {
                options.UseSqlServer(connectionString);
            }, ServiceLifetime.Transient, ServiceLifetime.Singleton);

            services.AddScoped<DungeonBot>();
            services.AddSingletonHostedService<DiscordSocketClientManagmentService>();

            services.AddSingleton<Logger>();
            services.AddSingleton<DiscordSocketClient>(p => new DiscordSocketClient(new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true
            }));

            services.AddSingleton<CommandService>();
            services.AddSingleton<CommandHandlingService>();

            Libraries.Configure(ffmpeg, youtubeDl, libOpus, libSodium);
            Youtube.Configure(youtubeApplicationName, youtubeKey);

            _configured = true;

        }
    }
}
