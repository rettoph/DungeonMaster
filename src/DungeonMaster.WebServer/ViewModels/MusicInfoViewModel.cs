using DungeonMaster.Library.Structs;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.ViewModels
{
    public class MusicInfoViewModel
    {
        public PlaybackRequest? NowPlaying { get; set; }
        public IEnumerable<PlaybackRequest> Queue { get; set; }
    }
}
