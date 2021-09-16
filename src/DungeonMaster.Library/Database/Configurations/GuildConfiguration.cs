using Discord.WebSocket;
using DungeonMaster.Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database.Configurations
{
    public class GuildConfiguration : BaseDiscordItemConfiguration<Guild, SocketGuild>
    {
        public override void Configure(EntityTypeBuilder<Guild> builder)
        {
            base.Configure(builder);

            builder.ToTable("Guilds");
            builder.Ignore(g => g.Music);
        }
    }
}
