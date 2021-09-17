using Discord.WebSocket;
using DungeonMaster.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Utilities
{
    public static class MusicManager
    {
        private static Dictionary<UInt64, MusicService> _musics = new Dictionary<UInt64, MusicService>();

        public static MusicService GetMusicService(SocketGuild guild)
        {
            if (_musics.TryGetValue(guild.Id, out MusicService value))
                return value;

            return _musics[guild.Id] = new MusicService(guild);
        }
    }
}
