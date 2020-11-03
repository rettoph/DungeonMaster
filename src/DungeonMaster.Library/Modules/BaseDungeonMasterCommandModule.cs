using Discord.Commands;
using DungeonMaster.Library.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonMaster.Library.Modules
{
    public class BaseDungeonMasterCommandModule : ModuleBase<ICommandContext>
    {
        protected DungeonContext dungeon;

        public GuildMaster GuildMaster => this.dungeon.GuildMasters.First(gm => gm.Guild == this.Context.Guild);

        public BaseDungeonMasterCommandModule(DungeonContext dungeon)
        {
            this.dungeon = dungeon;
        }
    }
}
