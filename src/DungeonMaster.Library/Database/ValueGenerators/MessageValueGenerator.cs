using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database.ValueGenerators
{
    public class MessageValueGenerator : ValueGenerator<IMessage>
    {
        private DiscordSocketClient _client;

        public override bool GeneratesTemporaryValues => true;

        public MessageValueGenerator(DiscordSocketClient client)
        {
            _client = client;
        }

        public override IMessage Next(EntityEntry entry)
            => (_client.GetChannel((UInt64)entry.Property("ChannelId").CurrentValue) as ITextChannel)
                    .GetMessageAsync((UInt64)entry.Property("MessageId").CurrentValue) as IMessage;
    }
}
