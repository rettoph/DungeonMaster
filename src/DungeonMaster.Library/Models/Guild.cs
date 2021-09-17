using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.Services;
using DungeonMaster.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonMaster.Library.Models
{
    public class Guild : BaseDiscordItem<SocketGuild>
    {
        public String Name { get; set; }

        public Lazy<MusicService> Music { get; private set; }

        public Guild()
        {
            this.Music = new Lazy<MusicService>(() => MusicManager.GetMusicService(this.SocketInstance.Value));
        }

        protected override SocketGuild GetSocketInstance(DiscordSocketClient client)
            => client.GetGuild(this.Id);

        public IEnumerable<SocketGuildChannel> GetChannels(ChannelType type)
            => this.GetChannels<SocketGuildChannel>(type);

        public IEnumerable<TSocketGuildChannel> GetChannels<TSocketGuildChannel>(ChannelType type)
            where TSocketGuildChannel : SocketGuildChannel
        {
            switch (type)
            {
                case ChannelType.Text:
                    return this.SocketInstance.Value.TextChannels as IEnumerable<TSocketGuildChannel>;
                case ChannelType.Voice:
                    return this.SocketInstance.Value.VoiceChannels as IEnumerable<TSocketGuildChannel>;
                case ChannelType.Category:
                    return this.SocketInstance.Value.CategoryChannels as IEnumerable<TSocketGuildChannel>;
                default:
                    return Enumerable.Empty<TSocketGuildChannel>();
            }
        }
    }
}
