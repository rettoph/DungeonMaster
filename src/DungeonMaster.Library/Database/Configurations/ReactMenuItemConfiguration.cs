using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.JsonConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database.Configurations
{
    public class ReactMenuItemConfiguration : IEntityTypeConfiguration<ReactMenuItem>
    {
        private DungeonContext _context;
        private IEmoteConverter _converter;

        public ReactMenuItemConfiguration(DungeonContext context)
        {
            _context = context;
            _converter = new IEmoteConverter();
        }

        public void Configure(EntityTypeBuilder<ReactMenuItem> builder)
        {
            builder.Property<IEmote>("Emote")
                .HasConversion<String>(
                    e => JsonConvert.SerializeObject(e, _converter),
                    e => JsonConvert.DeserializeObject<IEmote>(e, _converter));
        }
    }
}
