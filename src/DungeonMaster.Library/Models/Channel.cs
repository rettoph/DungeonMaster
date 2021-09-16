using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Models
{
    public class Channel<TSocketChannel> : BaseDiscordItem<TSocketChannel>
        where TSocketChannel : SocketChannel
    {
        protected override TSocketChannel GetSocketInstance(DiscordSocketClient client)
            => client.GetChannel(this.Id) as TSocketChannel;
    }
}
