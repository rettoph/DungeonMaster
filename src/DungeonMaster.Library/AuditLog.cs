using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.GuildManagers;
using Guppy;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library
{
    public sealed class AuditLog : Service
    {
        #region Private Fields
        private ILog _log;
        private PrimaryGuildManager _guild;
        private DiscordSocketClient _client;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _log);
            provider.Service(out _guild);
            provider.Service(out _client);
        }
        #endregion

        #region Helper Methods
        public void Log(String message, Color? color = null, IUser sender = null, ITextChannel target = null)
        {
            sender = sender ?? _client.CurrentUser;
            _log.Info($"{sender.Username} => {target?.Id}: {message}");

            (target ?? _guild.AuditLogChannel)?.SendMessageAsync(embed: new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    Name = sender.Username,
                    IconUrl = sender.GetAvatarUrl()
                },
                Description = message,
                Color = color ?? Color.Green
            }.Build());
        }
        #endregion
    }
}
