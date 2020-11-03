using Discord.Commands;
using DungeonMaster.Library.Interfaces;
using DungeonMaster.Library.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.ServiceLoaders
{
    internal sealed class CommandsServiceLoader : IServiceLoader
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<CommandService>();
            services.AddSingleton<CommandHandlingService>();
        }
    }
}
