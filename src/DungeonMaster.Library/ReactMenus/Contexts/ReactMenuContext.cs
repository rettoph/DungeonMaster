using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.ReactMenus.Contexts
{
    public class ReactMenuContext
    {
        public Guid? Id;
        public UInt64 ChannelId;
        public UInt64 MessageId;

        public String Name;
        public String Description;
        public List<ReactMenuItemContext> Items = new List<ReactMenuItemContext>();
    }
}
