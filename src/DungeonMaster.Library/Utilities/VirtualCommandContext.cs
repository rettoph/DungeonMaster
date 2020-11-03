using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Utilities
{
    public class VirtualCommandContext : ICommandContext
    {
        public IDiscordClient Client { get; set; }

        public IGuild Guild { get; set; }

        public IMessageChannel Channel { get; set; }

        public IUser User { get; set; }

        public IUserMessage Message { get; set; }
    }
}
