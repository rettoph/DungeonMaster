using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DungeonMaster.Library.ReactMenus;
using DungeonMaster.Library.ReactMenus.Contexts;
using DungeonMaster.Library.ServiceLists;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.GuildManagers
{
    public sealed class PrimaryGuildManager : GuildManager
    {
        #region Provate Fields
        private ServiceProvider _provider;
        private ReactMenuList _reactMenus;
        #endregion

        #region Public Properties
        public ReactMenu TogglePowerMenu { get; private set; }
        public ITextChannel AuditLogChannel { get; private set; }
        public IRole PassivePowerRole { get; private set; }
        public IRole ActivePowerRole { get; private set; }
        public ITextChannel PassiveAdminChannel { get; private set; }
        public ITextChannel TogglePowerChannel { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;

            provider.Service(out _reactMenus);
        }
        #endregion

        #region Helper Methods
        public override async Task StartAsync()
        {
            // Load the primary guild form discord
            this.Guild = this.client.GetGuild(this.config.PrimaryGuildId);
            await base.StartAsync();

            #region Roles
            // Ensure that the required roles exist...
            this.PassivePowerRole = this.GetOrCreateRoleAsync(
                id: ref this.config.PassivePowerRoleId, 
                name: this.config.DefaultPassivePowerRoleName, 
                color: Color.Red, 
                isHoisted: true, 
                isMentionable: true);

            this.ActivePowerRole = this.GetOrCreateRoleAsync(
                id: ref this.config.ActivePowerRoleId, 
                name: this.config.DefaultActivePowerRoleName, 
                color: Color.Red, 
                permissions: new GuildPermissions(administrator: true), 
                isMentionable: false);
            #endregion

            #region Channels
            // Create admin category if needed...
            this.AdministrationCategoryChannel = this.GetOrCreateCategoryChannel(
                id: ref this.config.AdministrationCategoryChannelId,
                name: this.config.DefaultAdministrationCategoryChannelName, 
                func: gcp =>
                {
                    gcp.PermissionOverwrites = new List<Overwrite>(new Overwrite[] {
                    new Overwrite(
                        targetId: this.Guild.EveryoneRole.Id,
                        targetType: PermissionTarget.Role,
                        permissions: new OverwritePermissions(0, ChannelPermissions.Category.RawValue))
                    });
                });
            
            this.AuditLogChannel = this.GetOrCreateTextChannel(
                id: ref this.config.AuditLogChannelId,
                name: this.config.DefaultAuditLogChannelName, 
                func: tcp =>
                {
                    tcp.CategoryId = this.AdministrationCategoryChannel.Id;
                });

            this.TogglePowerChannel = this.GetOrCreateTextChannel(
                id: ref this.config.TogglePowerChannelId,
                name: this.config.DefaultTogglePowerChannelName, 
                func: tcp =>
                {
                    tcp.CategoryId = this.AdministrationCategoryChannel.Id;
                    tcp.PermissionOverwrites = new List<Overwrite>(new Overwrite[] {
                        new Overwrite(
                            targetId: this.PassivePowerRole.Id, 
                            targetType: PermissionTarget.Role, 
                            permissions: new OverwritePermissions(
                                viewChannel: PermValue.Allow, 
                                addReactions: PermValue.Allow, 
                                readMessageHistory: PermValue.Allow))
                    });
                });
            #endregion

            #region Reaction Menus
            this.config.ReactMenus.ForEach(rmc => _reactMenus.Create((rm, p, d) =>
            { // Load all creact menus...
                if(rm.TryLoadContext(rmc))
                    this.log.Log($"Re-established link with ReactMenu<'{rm.Name}'>.", Color.Blue);
                else
                    this.log.Log($"Error Re-establishing link with ReactMenu<'{rm.Name}'>...", Color.Red);
            }));

            this.config.TogglePowerMenuId = (this.TogglePowerMenu = _reactMenus.GetByIdOrCreate(this.config.TogglePowerMenuId, () => new ReactMenuContext()
            {
                ChannelId = this.TogglePowerChannel.Id,
                Name = "Toggle Power Menu",
                Description = "Greetings power user. React to this message and toggle wealth beyond your wildest dreams.",
                Items = new List<ReactMenuItemContext>(new ReactMenuItemContext[] {
                    new ReactMenuItemContext()
                    {
                        Name = "Admin",
                        Description = "Full guild power.",
                        Icon = new Emoji("✅")
                    }
                })
            }, this.TogglePowerChannel)).Id;

            this.TogglePowerMenu.Items["Admin"].OnToggled += async (m, mi, reaction) =>
            {
                var user = this.Guild.GetUser(reaction.UserId);

                if (user.Roles.Contains(this.ActivePowerRole))
                {
                    await user.RemoveRoleAsync(this.ActivePowerRole);
                    this.log.Log($"Became passive power user.", Color.Green, user);
                }
                else
                {
                    await user.AddRoleAsync(this.ActivePowerRole);
                    this.log.Log($"Became active power user.", Color.Orange, user);
                }
            };
            #endregion

            this.config.Flush();
        }
        #endregion
    }
}
