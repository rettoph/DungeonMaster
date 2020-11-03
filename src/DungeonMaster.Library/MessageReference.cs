using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DungeonMaster.Library
{
    public class MessageReference
    {
        #region Private Fields
        private DiscordSocketClient _client;
        #endregion

        #region Public Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 Id { get; set; }
        public UInt64 MessageId { get; set; }
        public UInt64 ChannelId { get; set; }
        #endregion

        #region Constructor
        public MessageReference(DungeonContext context)
        {
            _client = context.GetService<DiscordSocketClient>();
        }
        public MessageReference(IMessage message)
        {
            this.ChannelId = message.Channel.Id;
            this.MessageId = message.Id;
        }
        #endregion

        #region Helper Methods
        public IMessage GetMessage()
            => (_client.GetChannel(this.ChannelId) as ITextChannel).GetMessageAsync(this.MessageId).Result;
        #endregion
    }
}
