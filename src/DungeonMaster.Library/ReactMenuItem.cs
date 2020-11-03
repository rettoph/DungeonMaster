using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DungeonMaster.Library
{
    public class ReactMenuItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 Id { get; set; }

        public ReactMenu ReactMenu { get; set; }

        public IEmote Emote { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }
    }
}
