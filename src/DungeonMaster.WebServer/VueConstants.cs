using DungeonMaster.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer
{
    public static class VueConstants
    {
        public static readonly VueConfig GuildsIndex = new VueConfig("guilds-index", "/js/guilds-index.js");
        public static readonly VueConfig GuildsCategories = new VueConfig("guilds-categories", "/js/guilds-categories.js");
        public static readonly VueConfig MusicIndex = new VueConfig("music-index", "/js/music-index.js");
    }
}
