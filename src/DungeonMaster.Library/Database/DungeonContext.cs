using DungeonMaster.Library.Database.Configurations;
using DungeonMaster.Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database
{
    public class DungeonContext : DbContext
    {
        public DbSet<LogMessage> LogMessages { get; set; }
        public DbSet<Guild> Guilds { get; set; }

        public DungeonContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration<Guild>(new GuildConfiguration());
        }
    }
}
