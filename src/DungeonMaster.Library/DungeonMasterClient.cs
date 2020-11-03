using Discord;
using Discord.WebSocket;
using DungeonMaster.Extensions.System;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Extensions.Microsoft.EntityFramework;
using DungeonMaster.Library.Interfaces;
using DungeonMaster.Library.Services;
using DungeonMaster.Library.Utilities;
using DungeonMaster.Utilities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library
{
    public class DungeonMasterClient
    {
        #region Private Fields
        private Config _config;
        private DiscordSocketClient _discord;
        private DungeonContext _context;
        private ILogger<DungeonMasterClient> _logger;
        private ILogger<DiscordSocketClient> _discordLogger;
        #endregion

        #region Constructor
        public DungeonMasterClient(
            Config config, 
            DiscordSocketClient discord, 
            DungeonContext context, 
            ILogger<DungeonMasterClient> logger,
            ILogger<DiscordSocketClient> discordLogger)
        {
            _config = config;
            _discord = discord;
            _context = context;
            _logger = logger;
            _discordLogger = discordLogger;
        }
        #endregion

        #region Helper Methods
        public async void StartAsync()
        {
            _logger.LogInformation($"Starting...");

            _discord.GuildAvailable += this.HandleGuildAvailable;
            _discord.Log += this.HandleLog;

            await _discord.LoginAsync(Discord.TokenType.Bot, _config.BotToken);
            await _discord.StartAsync();

            await _context.GetService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }
        #endregion

        #region Event Handlers
        private async Task HandleGuildAvailable(SocketGuild guild)
        {
            try
            {
                var gm = _context.GuildMasters.FindOrCreate(
                    filter: gm => gm.Guild == guild,
                    builder: gm =>
                    {
                        gm.Guild = guild;
                    });

                await gm.InitializeAsync();

                lock(_context)
                    _context.SaveChanges();
            }
            catch(Exception e)
            {
                _logger.LogCritical(e.ToString());
            }
        }

        private Task HandleLog(LogMessage arg)
        {
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    _discordLogger.LogCritical(arg.Message);
                    break;
                case LogSeverity.Error:
                    _discordLogger.LogError(arg.Message);
                    break;
                case LogSeverity.Warning:
                    _discordLogger.LogWarning(arg.Message);
                    break;
                case LogSeverity.Info:
                    _discordLogger.LogInformation(arg.Message);
                    break;
                case LogSeverity.Verbose:
                    _discordLogger.LogTrace(arg.Message);
                    break;
                case LogSeverity.Debug:
                    _discordLogger.LogDebug(arg.Message);
                    break;
            }

            return Task.CompletedTask;
        }
        #endregion

        #region Static Methods
        public static IServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();

            foreach (IServiceLoader loader in AssemblyHelper.Types.GetTypesAssignableFrom<IServiceLoader>().Where(t => t.IsClass).Select(t => (IServiceLoader)Activator.CreateInstance(t)))
                loader.RegisterServices(services);

            return services.BuildServiceProvider();
        }
        #endregion
    }
}
