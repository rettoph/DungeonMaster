using Discord.WebSocket;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.GuildManagers
{
    public sealed class StorageGuildManager : GuildManager
    {
        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);
        }
        #endregion

        public override Task StartAsync()
        {
            // Load the primary guild form discord
            this.Guild = this.client.GetGuild(this.config.StorageGuildId);

            return base.StartAsync();
        }
    }
}
