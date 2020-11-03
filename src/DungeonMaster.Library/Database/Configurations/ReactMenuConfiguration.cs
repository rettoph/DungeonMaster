using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.Database.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database.Configurations
{
    public class ReactMenuConfiguration : IEntityTypeConfiguration<ReactMenu>
    {
        private DungeonContext _context;

        public ReactMenuConfiguration(DungeonContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<ReactMenu> builder)
        {
            // builder.Property<MessageReference>("Message")
            //     .HasConversion<SocketMessage>(
            //         m => m,
            //         m => m);
        }
    }
}
