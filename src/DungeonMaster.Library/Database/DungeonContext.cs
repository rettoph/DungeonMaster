using Discord.WebSocket;
using DungeonMaster.Library.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database
{
    public sealed class DungeonContext : DbContext
    {
        #region Private Fields
        private DiscordSocketClient _client;
        #endregion

        #region Public Properties
        public DbSet<MessageReference> MessageReferences { get; set; }
        public DbSet<GuildMaster> GuildMasters { get; set; }
        public DbSet<ReactMenu> ReactMenus { get; set; }
        public DbSet<ReactMenuItem> ReactMenuItems { get; set; }
        #endregion

        public DungeonContext(DbContextOptions<DungeonContext> options, DiscordSocketClient client) : base(options)
        {
            _client = client;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration<MessageReference>(new MessageReferenceConfiguration(this));
            builder.ApplyConfiguration<GuildMaster>(new GuildMasterConfiguration(this));
            builder.ApplyConfiguration<ReactMenu>(new ReactMenuConfiguration(this));
            builder.ApplyConfiguration<ReactMenuItem>(new ReactMenuItemConfiguration(this));
        }
    }
}
