using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
            => ReplyAsync("pong!");
    }
}
