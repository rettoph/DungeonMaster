using DungeonMaster.Library.Enums;
using DungeonMaster.Library.Extensions.DependencyInjection;
using DungeonMaster.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Utilities
{
    public class Logger
    {
        private IServiceProvider _provider;

        public Logger(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Log(String message, LogLevel level, UInt64? guildId, LogMessageType type = LogMessageType.Audit)
        {
            _provider.UsingContextAsync(async context =>
            {
                context.LogMessages.Add(new LogMessage()
                {
                    GuildId = guildId,
                    Message = message,
                    Level = level,
                    Type = type,
                    Timestamp = DateTime.Now
                });

                await context.SaveChangesAsync();
            });
        }

        public void Verbose(String message, UInt64? guildId = default, LogMessageType type = LogMessageType.Audit)
            => this.Log(message, LogLevel.Verbose, guildId, type);

        public void Debug(String message, UInt64? guildId = default, LogMessageType type = LogMessageType.Audit)
            => this.Log(message, LogLevel.Debug, guildId, type);

        public void Info(String message, UInt64? guildId = default, LogMessageType type = LogMessageType.Audit)
            => this.Log(message, LogLevel.Info, guildId, type);

        public void Warning(String message, UInt64? guildId = default, LogMessageType type = LogMessageType.Audit)
            => this.Log(message, LogLevel.Warning, guildId, type);

        public void Error(String message, UInt64? guildId = default, LogMessageType type = LogMessageType.Audit)
            => this.Log(message, LogLevel.Error, guildId, type);

        public void Critical(String message, UInt64? guildId = default, LogMessageType type = LogMessageType.Audit)
            => this.Log(message, LogLevel.Critical, guildId, type);
    }
}
