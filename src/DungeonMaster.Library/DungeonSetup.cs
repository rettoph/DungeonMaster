using Discord;
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
        public static void ConfigureServices(IServiceCollection services, String connectionString, String ffmpeg, String youtubeDl, String libOpus, String libSodium)
        {
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

            Libraries.Configure(ffmpeg, youtubeDl, libOpus, libSodium);
        }


    }
}
