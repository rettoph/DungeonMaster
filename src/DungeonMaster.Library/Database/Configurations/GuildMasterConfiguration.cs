using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database.Configurations
{
    public class GuildMasterConfiguration : IEntityTypeConfiguration<GuildMaster>
    {
        private DungeonContext _context;
        private DiscordSocketClient _client;

        public GuildMasterConfiguration(DungeonContext context)
        {
            _context = context;
            _client = _context.GetService<DiscordSocketClient>();
        }

        public void Configure(EntityTypeBuilder<GuildMaster> builder)
        {
            builder.HasIndex("Guild");

            builder.Property<SocketGuild>("Guild")
                .HasConversion<UInt64?>(
                    v => v.Id,
                    v => _client.GetGuild(v.Value));

            builder.Property<ICategoryChannel>("AdminitrationCategoryChannel")
                .HasConversion<UInt64>(
                    v => v.Id,
                    v => _client.GetChannel(v) as ICategoryChannel);

            builder.Property<ITextChannel>("AuditLogChannel")
                .HasConversion<UInt64>(
                    v => v.Id,
                    v => _client.GetChannel(v) as ITextChannel);

            builder.Property<ITextChannel>("TogglePowerChannel")
                .HasConversion<UInt64>(
                    v => v.Id,
                    v => _client.GetChannel(v) as ITextChannel);
        }
    }
}
