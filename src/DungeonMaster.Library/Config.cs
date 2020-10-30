using DungeonMaster.Library.JsonConverters;
using DungeonMaster.Library.ReactMenus;
using DungeonMaster.Library.ReactMenus.Contexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonMaster.Library
{
    /// <summary>
    /// Primary class maintaining internal configurations
    /// and bot related settings. To update values, run bot
    /// once and edit the config.json file.
    /// </summary>
    public class Config
    {
        #region Constants
        private const String FilePath = "./config.json";
        #endregion

        #region Public Fields
        public String BotToken;
        public UInt64 PrimaryGuildId;
        public UInt64 StorageGuildId;

        public String DefaultPassivePowerRoleName = "Admin";
        public UInt64? PassivePowerRoleId;

        public String DefaultActivePowerRoleName = "ActiveAdmin";
        public UInt64? ActivePowerRoleId;

        public String DefaultAdministrationCategoryChannelName = "Administration";
        public UInt64? AdministrationCategoryChannelId;

        public String DefaultAuditLogChannelName = "audit-log";
        public UInt64? AuditLogChannelId;

        public String DefaultTogglePowerChannelName = "toggle-power";
        public UInt64? TogglePowerChannelId;
        public Guid? TogglePowerMenuId;

        public HashSet<ReactMenuContext> ReactMenus = new HashSet<ReactMenuContext>();
        #endregion

        #region Helper Methods
        public void Flush()
        {
            using (StreamWriter writer = File.CreateText(Config.FilePath))
            {
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented, new IEmoteConverter()));
                writer.Flush();
            }
        }
        #endregion

        #region Static Helper Methods
        /// <summary>
        /// Acts as the primary "constructor" that will auto import
        /// the config file as needed.
        /// </summary>
        /// <returns></returns>
        internal static Config BuildFromFile()
        {
            if(File.Exists(Config.FilePath))
            { // Import file...
                using (FileStream stream = File.OpenRead(Config.FilePath))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return JsonConvert.DeserializeObject<Config>(reader.ReadToEnd(), new IEmoteConverter());
                    }
                }
            }
            else
            { // Create a new file...
                var config = new Config();
                config.Flush();
                throw new Exception($"Please populate 'config.json' before proceeding.");
            }
        }
        #endregion
    }
}
