using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using log4net;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy;
using DungeonMaster.Library.Extensions.log4net;

namespace DungeonMaster.Library.ServiceLoaders
{
    [AutoLoad]
    internal sealed class OutputServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            // Configure & add log4net services...
            services.AddFactory<ILog>(p => LogManager.GetLogger(typeof(GuppyLoader)));
            services.AddFactory<ILoggerRepository>(p => p.GetService<ILog>().Logger.Repository);
            services.AddFactory<Hierarchy>(p => (Hierarchy)p.GetService<ILoggerRepository>());

            services.AddSingleton<ILog>();
            services.AddSingleton<ILoggerRepository>();
            services.AddSingleton<Hierarchy>();

            services.AddConfiguration<ILog>((l, p, s) =>
            {
                l.SetLevel(Level.Verbose);
                l.ConfigureFileAppender($"logs\\{DateTime.Now.ToString("yyy-MM-dd")}.txt")
                    .ConfigureManagedColoredConsoleAppender(new ManagedColoredConsoleAppender.LevelColors()
                    {
                        BackColor = ConsoleColor.Red,
                        ForeColor = ConsoleColor.White,
                        Level = Level.Fatal
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Red,
                        Level = Level.Error
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Yellow,
                        Level = Level.Warn
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.White,
                        Level = Level.Info
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Magenta,
                        Level = Level.Debug
                    }, new ManagedColoredConsoleAppender.LevelColors()
                    {
                        ForeColor = ConsoleColor.Cyan,
                        Level = Level.Verbose
                    });

                // Mark as configured...
                ((Hierarchy)l.Logger.Repository).Configured = true;
            }, -10);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {

        }
    }
}
