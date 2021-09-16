using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Models
{
    public abstract class BaseDiscordItem<TSocketInstance>
    {
        public UInt64 Id { get; set; }

        public Lazy<TSocketInstance> SocketInstance { get; private set; }

        public BaseDiscordItem()
        {
            this.SocketInstance = new Lazy<TSocketInstance>(this.GetSocketInstance);
        }

        private TSocketInstance GetSocketInstance()
            => this.GetSocketInstance(DungeonBot.ClientInstance);

        protected abstract TSocketInstance GetSocketInstance(DiscordSocketClient client);
    }
}
