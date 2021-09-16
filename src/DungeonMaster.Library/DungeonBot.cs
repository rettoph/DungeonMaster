using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DungeonMaster.Library.Models;
using LogMessage = DungeonMaster.Library.Models.LogMessage;
using DungeonMaster.Library.Utilities;
using DungeonMaster.Library.Extensions.DependencyInjection;
using DungeonMaster.Library.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DungeonMaster.Library
{
    public sealed class DungeonBot
    {
        internal static DiscordSocketClient ClientInstance { get; set; }
        internal static Logger Logger { get; set; }

        #region Private Fields
        private DiscordSocketClient _client;
        private IConfiguration _configuration;
        private DungeonContext _context;
        #endregion

        #region Public Properties
        public String AuthorizeUrl { get; private set; }

        public DbSet<Guild> Guilds => _context.Guilds;
        #endregion

        #region Constructor
        public DungeonBot(DiscordSocketClient client, IConfiguration configuration, DungeonContext context)
        {
            _client = client;
            _configuration = configuration;
            _context = context;

            String applicationId = _configuration["Discord:ApplicationId"];
            String permissions = _configuration["Discord:Bot:Permissions"];

            this.AuthorizeUrl = $"https://discord.com/api/oauth2/authorize?client_id={applicationId}&permissions={permissions}&scope=bot";
        }
        #endregion

        #region Helper Methods
        public SocketUser GetUser(UInt64 userId)
        {
            return DungeonBot.ClientInstance.GetUser(userId);
        }
        #endregion
    }
}
