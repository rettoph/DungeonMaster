using DungeonMaster.Library.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Models
{
    public class LogMessage
    {
        public Int32 Id { get; set; }
        public UInt64? GuildId { get; set; }
        public DateTime Timestamp { get; set; }
        public String Message { get; set; }
        public LogLevel Level { get; set; }
        public LogMessageType Type { get; set; }
    }
}
