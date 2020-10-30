using Discord.WebSocket;
using Guppy;
using Guppy.DependencyInjection;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Guppy.Extensions.DependencyInjection;
using Guppy.Events.Delegates;
using Discord;
using System.Linq;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Appender;

namespace DungeonMaster.Library.GuildManagers
{
    public abstract class GuildManager : Service
    {
        #region Protected Properties
        protected DiscordSocketClient client { get; private set; }
        protected Config config { get; private set; }
        protected ICategoryChannel AdministrationCategoryChannel;
        protected AuditLog log { get; private set; }
        #endregion

        #region Public Fields
        public SocketGuild Guild { get; protected set; }
        #endregion

        #region Events
        public OnEventDelegate<GuildManager, SocketGuildUser> UserJoined;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.log = provider.GetService<AuditLog>();
            this.client = provider.GetService<DiscordSocketClient>();
            this.config = provider.GetService<Config>();

            this.client.UserJoined += this.HandleUserJoined;
        }
        #endregion

        #region Helper Methods
        public virtual async Task StartAsync()
        {
            while (!this.Guild.IsSynced)
                await Task.Delay(100);
        }

        /// <summary>
        /// Get or create an object & automatically audit it as needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getter"></param>
        /// <param name="creater"></param>
        /// <returns></returns>
        public T GetOrCreate<T>(ref UInt64? id, Func<UInt64, T> getter, Func<Task<T>> creator, Func<T, String> audit = null, Color? color = null)
            where T : class, ISnowflakeEntity
        {
            T instance;
            if(id == null || (instance = getter(id.Value)) == default)
            { // Create and audit...
                instance = creator().GetAwaiter().GetResult();

                id = instance.Id;
                this.log.Log(audit?.Invoke(instance), color);
            }

            return instance;
        }

        public TChannel GetOrCreateChannel<TChannel>(ref UInt64? id, Func<Task<TChannel>> creator)
            where TChannel : class, IChannel
                => this.GetOrCreate(
                    id: ref id,
                    getter: id => this.Guild.GetChannel(id) as TChannel,
                    creator: creator,
                    audit: i => $"Created <#{i.Id}> Channel<{typeof(TChannel).Name}>.",
                    color: Color.Green);

        public ICategoryChannel GetOrCreateCategoryChannel(ref UInt64? id, String name, Action<GuildChannelProperties> func = null)
            => this.GetOrCreateChannel<ICategoryChannel>(ref id, async () => await this.Guild.CreateCategoryChannelAsync(name: name, func: func));
        public ITextChannel GetOrCreateTextChannel(ref UInt64? id, String name, Action<TextChannelProperties> func)
            => this.GetOrCreateChannel<ITextChannel>(ref id, async () => await this.Guild.CreateTextChannelAsync(name: name, func: func));
        public IVoiceChannel GetOrCreateVoiceChannel(ref UInt64? id, String name, Action<VoiceChannelProperties> func)
            => this.GetOrCreateChannel<IVoiceChannel>(ref id, async () => await this.Guild.CreateVoiceChannelAsync(name: name, func: func));

        public IRole GetOrCreateRoleAsync(ref UInt64? id, String name, GuildPermissions? permissions = null, Color? color = null, Boolean isHoisted = false, Boolean isMentionable = false)
            => this.GetOrCreate<IRole>(
                id: ref id,
                getter: id => this.Guild.GetRole(id),
                creator: async () => await this.Guild.CreateRoleAsync(name, permissions, color, isHoisted, isMentionable) as IRole,
                audit: i => $"Created {i.Mention} role.",
                color: Color.Green);

        public async Task AddChannelPermissionOverwriteAsync(IGuildChannel channel, IRole role, OverwritePermissions permissions)
        {
            if(!channel.GetPermissionOverwrite(role).GetValueOrDefault().Equals(permissions))
            { // Only proceed id the perms have changed...
                await channel.AddPermissionOverwriteAsync(role, permissions);

                this.log.Log($"Updated <#{channel.Id}> permissions for {role.Mention} => {permissions.ToString()}", Color.Orange);
            }
        }
        #endregion

        #region Event Handlers
        private Task HandleUserJoined(SocketGuildUser arg)
        {
            if (arg.Guild.Id == this.Guild.Id)
                this.UserJoined?.Invoke(this, arg);

            return Task.CompletedTask;
        }
        #endregion
    }
}
