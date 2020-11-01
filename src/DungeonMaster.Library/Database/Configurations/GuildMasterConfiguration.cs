using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database.Configurations
{
    public class GuildMasterConfiguration : IEntityTypeConfiguration<GuildMaster>
    {
        private DiscordSocketClient _client;

        public GuildMasterConfiguration(DiscordSocketClient client)
        {
            _client = client;
        }

        public void Configure(EntityTypeBuilder<GuildMaster> builder)
        {
            builder.Ignore("Id");
            builder.Ignore("ServiceContext");
            builder.Ignore("InitializationStatus");

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
