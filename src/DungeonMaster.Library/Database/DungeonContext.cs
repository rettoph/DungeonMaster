using Discord.WebSocket;
using DungeonMaster.Library.Database.Configurations;
using Guppy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database
{
    public sealed class DungeonContext : DbContext
    {
        #region Private Fields
        private Config _config;
        private DiscordSocketClient _client;
        #endregion

        #region Public Properties
        public DbSet<GuildMaster> GuildMasters { get; set; }
        #endregion

        public DungeonContext(Config config, DiscordSocketClient client)
        {
            _config = config;
            _client = client;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(_config.ConnectionInfo.ToString());

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // builder.Entity<Service>(smb =>
            // {
            //     smb.Ignore("ServiceContext");
            //     smb.Ignore("Id");
            //     smb.HasNoKey();
            // });

            builder.ApplyConfiguration<GuildMaster>(new GuildMasterConfiguration(_client));
        }
    }
}
