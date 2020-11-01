using Discord.WebSocket;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Extensions.Microsoft.EntityFramework;
using Guppy.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Constructor
        public DungeonMasterClient(
            Config config, 
            DiscordSocketClient discord, 
            DungeonContext context, 
            ILogger<DungeonMasterClient> logger)
        {
            _config = config;
            _discord = discord;
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Helper Methods
        public async void StartAsync()
        {
            await _discord.LoginAsync(Discord.TokenType.Bot, _config.BotToken);
            await _discord.StartAsync();

            _discord.GuildAvailable += this.HandleGuildAvailable;

            await Task.Delay(-1);
        }
        #endregion

        #region Event Handlers
        private Task HandleGuildAvailable(SocketGuild guild)
        {
            try
            {
                _context.GuildMasters.FindOrCreate(
                    filter: gm => gm.Guild == guild,
                    builder: gm =>
                    {
                        gm.Guild = guild;
                    }).Validate();

                _context.SaveChanges();
            }
            catch(Exception e)
            {
                _logger.LogCritical(e.ToString());
            }

            return Task.CompletedTask;
        }
        #endregion
    }
}
