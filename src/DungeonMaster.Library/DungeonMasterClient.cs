using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.GuildManagers;
using DungeonMaster.Library.ServiceLists;
using Guppy;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library
{
    /// <summary>
    /// Represents the primary dungeon master client instance.
    /// 
    /// This is the main interface point to create & run the bot.
    /// </summary>
    public sealed class DungeonMasterClient : Service
    {
        #region Private Fields
        private ServiceProvider _provider;
        private Config _config;
        private DiscordSocketClient _client;
        private ReactMenuList _reactMenus;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;
            provider.Service(out _config);
            provider.Service(out _client);
            provider.Service(out _reactMenus);
        }
        #endregion

        #region Helper Methods
        public async Task StartAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _config.BotToken);
            await _client.StartAsync();

            _client.Connected += this.HandleClientConnected;
            _client.ReactionAdded += this.HandleReactionChanged;
            _client.ReactionRemoved += this.HandleReactionChanged;

            await Task.Delay(-1);
        }
        #endregion

        #region Event Handlers
        private async Task HandleClientConnected()
        {
            await _provider.GetService<PrimaryGuildManager>().StartAsync();
            await _provider.GetService<StorageGuildManager>().StartAsync();

            await Task.CompletedTask;
        }

        private Task HandleReactionChanged(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if(arg3.User.Value.Id != _client.CurrentUser.Id)
                _reactMenus.FirstOrDefault(rm => rm.MessageId == arg3.MessageId)?.ToggleReaction(arg3);

            return Task.CompletedTask;
        }
        #endregion
    }
}