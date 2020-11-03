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
    public class MessageReferenceConfiguration : IEntityTypeConfiguration<MessageReference>
    {
        private DungeonContext _context;

        public MessageReferenceConfiguration(DungeonContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<MessageReference> builder)
        {
            builder.HasIndex(m => new { m.ChannelId, m.MessageId });

            builder.Property<IMessage>("Message")
                .ValueGeneratedOnAddOrUpdate()
                .HasValueGenerator<MessageValueGenerator>();

            builder.Ignore("Message");
        }
    }
}
