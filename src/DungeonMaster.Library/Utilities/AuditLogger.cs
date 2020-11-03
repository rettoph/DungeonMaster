using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Utilities
{
    public sealed class AuditLogger
    {
        #region Private Fields
        private GuildMaster _guildMaster;
        #endregion

        #region Constructors
        public AuditLogger(GuildMaster guildMaster)
        {
            _guildMaster = guildMaster;
        }
        #endregion

        #region Helper Methods
        public void Log(String message, IGuildUser sender = null, Color? color = null)
        {
            sender ??= _guildMaster.Guild.CurrentUser;
            color ??= Color.Green;

            _guildMaster.AuditLogChannel.SendMessageAsync(embed: this.GetEmbed(message, sender, color.Value));
        }

        public Embed GetEmbed(String message, IGuildUser sender, Color color)
        {
            var builder = new EmbedBuilder();

            builder.Author = new EmbedAuthorBuilder()
            {
                Name = sender.Username,
                IconUrl = sender.GetAvatarUrl()
            };

            builder.Description = message;
            builder.Color = color;
            builder.WithCurrentTimestamp();

            return builder.Build();
        }
        #endregion
    }
}
