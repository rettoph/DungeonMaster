using DungeonMaster.Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database.Configurations
{
    public class BaseDiscordItemConfiguration<TDiscordItem, TSocketInstance> : IEntityTypeConfiguration<TDiscordItem>
        where TDiscordItem : BaseDiscordItem<TSocketInstance>
    {
        public virtual void Configure(EntityTypeBuilder<TDiscordItem> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Ignore(d => d.SocketInstance);
        }
    }
}
