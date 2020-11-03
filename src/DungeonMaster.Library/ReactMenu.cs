using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Utilities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library
{
    public class ReactMenu
    {
        #region Private Fields
        private DungeonContext _context;
        #endregion

        #region Public Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public MessageReference MessageReference { get; set; }

        public GuildMaster GuildMaster { get; set; }

        [NotMapped]
        public IMessage Message
        { 
            get => this.MessageReference.GetMessage();
            set => this.MessageReference = new MessageReference(value);
        }

        public ICollection<ReactMenuItem> Items { get; set; }
        #endregion

        #region Constructor
        public ReactMenu(DungeonContext context)
        {
            _context = context;

            this.Items = new List<ReactMenuItem>();
        }
        #endregion

        #region Helper Methods
        public ReactMenuItem CreateItem(IEmote emote, String name, String description, String commands)
        {
            var rmi = _context.GetService<ReactMenuItem>();
            rmi.ReactMenu = this;
            rmi.Emote = emote;
            rmi.Name = name;
            rmi.Description = description;
            rmi.Command = commands;
            this.Items.Add(rmi);

            this.GuildMaster.AuditLogger.Log($"Created ReactMenuItem<{rmi.Id}>({rmi.Name}) for ReactMenu<{this.Id}>");

            return rmi;
        }
        #endregion
    }
}
