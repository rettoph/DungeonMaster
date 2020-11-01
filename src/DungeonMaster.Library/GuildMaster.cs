using Discord;
using Discord.WebSocket;
using Guppy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DungeonMaster.Library
{
    public class GuildMaster : Service
    {
        [Key]
        public SocketGuild Guild { get; set; }

        public ICategoryChannel AdminitrationCategoryChannel { get; set; }

        public ITextChannel AuditLogChannel { get; set; }

        public ITextChannel TogglePowerChannel { get; set; }

        #region Helper Methods
        public void Validate()
        {

        }
        #endregion
    }
}
