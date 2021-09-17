using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.ViewModels
{
    public class MusicInfoViewModel
    {
        public Video NowPlaying { get; set; }
        public IEnumerable<Video> Queue { get; set; }
    }
}
